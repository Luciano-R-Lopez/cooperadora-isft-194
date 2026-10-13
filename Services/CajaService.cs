using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class CajaService : ICajaService
    {
        private static readonly Lazy<CajaService> _instancia = new(() =>
            new CajaService(CajaRepository.Instancia, MovimientoRepository.Instancia,
                            CajaTotalMedioPagoRepository.Instancia, AuditoriaService.Instancia));
        public static CajaService Instancia => _instancia.Value;

        private readonly ICajaRepository _cajas;
        private readonly IMovimientoRepository _movimientos;
        private readonly ICajaTotalMedioPagoRepository _totales;
        private readonly IAuditoriaService _auditoria;

        private CajaService(ICajaRepository cajas, IMovimientoRepository movimientos,
                            ICajaTotalMedioPagoRepository totales, IAuditoriaService auditoria)
        {
            _cajas = cajas;
            _movimientos = movimientos;
            _totales = totales;
            _auditoria = auditoria;
        }

        public CajaDto? ObtenerActiva()
        {
            var caja = _cajas.ObtenerActiva();
            return caja is null ? null : CajaMapper.ADto(caja);
        }

        public IReadOnlyList<CajaDto> ObtenerTodas() =>
            _cajas.ObtenerTodos()
                  .OrderByDescending(c => c.FechaApertura)
                  .Select(CajaMapper.ADto)
                  .ToList();

        public CajaDto Abrir(AbrirCajaDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (_cajas.ObtenerActiva() is not null)
                throw new ReglaNegocioException("Ya hay una caja abierta.");

            // La Caja valida su monto inicial.
            var caja = _cajas.Agregar(new Caja(dto.MontoInicial, DateTime.Now));
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Caja), caja.Id);

            return CajaMapper.ADto(caja);
        }

        public ResumenCajaDto Cerrar(CerrarCajaDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var caja = ObtenerCajaAbierta();
            caja.CerrarCaja(DateTime.Now, dto.MontoTotal);
            _cajas.Actualizar(caja);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Caja), caja.Id);

            return CalcularResumen(caja, dto.MontoTotal);
        }

        public ResumenCajaDto ObtenerResumenActivo(decimal? montoDeclarado) =>
            CalcularResumen(ObtenerCajaAbierta(), montoDeclarado);

        private Caja ObtenerCajaAbierta() =>
            _cajas.ObtenerActiva() ?? throw new NoEncontradoException("No hay una caja abierta.");

        /// <summary>Diferencia = monto declarado - monto esperado (inicial + saldo neto de cada medio de pago).</summary>
        private ResumenCajaDto CalcularResumen(Caja caja, decimal? montoDeclarado)
        {
            var movimientos = _movimientos.ObtenerPorCaja(caja);
            var totalIngresos = movimientos.Where(m => m.Tipo == TipoMovimiento.Ingreso).Sum(m => m.Monto);
            var totalEgresos = movimientos.Where(m => m.Tipo == TipoMovimiento.Egreso).Sum(m => m.Monto);

            var totales = _totales.ObtenerPorCaja(caja);
            var montoEsperado = caja.MontoInicial + totales.Sum(t => t.Monto);
            decimal? diferencia = montoDeclarado.HasValue ? montoDeclarado.Value - montoEsperado : null;

            var totalesPorMedio = totales
                .OrderBy(t => t.MedioPago.Nombre)
                .Select(t => new TotalMedioPagoDto(t.MedioPago.Nombre, t.Monto))
                .ToList();

            return new ResumenCajaDto(
                caja.Id, caja.MontoInicial, totalIngresos, totalEgresos, montoEsperado,
                montoDeclarado, diferencia, totalesPorMedio);
        }
    }
}
