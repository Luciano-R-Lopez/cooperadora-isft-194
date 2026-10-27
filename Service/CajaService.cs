using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Service
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
        private readonly Func<DateTime> _reloj;

        internal CajaService(ICajaRepository cajas, IMovimientoRepository movimientos,
                             ICajaTotalMedioPagoRepository totales, IAuditoriaService auditoria,
                             Func<DateTime>? reloj = null)
        {
            _cajas = cajas;
            _movimientos = movimientos;
            _totales = totales;
            _auditoria = auditoria;
            _reloj = reloj ?? (() => DateTime.Now);
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
            var caja = _cajas.Agregar(new Caja(dto.MontoInicial, _reloj()));
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Caja), caja.Id,
                                 $"Apertura con {caja.MontoInicial:C2}");

            return CajaMapper.ADto(caja);
        }

        public ResumenCajaDto Cerrar(CerrarCajaDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var caja = ObtenerCajaAbierta();
            caja.CerrarCaja(_reloj(), dto.MontoTotal);
            _cajas.Actualizar(caja);

            var resumen = CalcularResumen(caja, dto.MontoTotal);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Caja), caja.Id,
                                 $"Cierre: esperado {resumen.MontoEsperado:C2}, contado {dto.MontoTotal:C2}, diferencia {resumen.Diferencia:C2}");

            return resumen;
        }

        public ResumenCajaDto ObtenerResumenActivo(decimal? montoDeclarado) =>
            CalcularResumen(ObtenerCajaAbierta(), montoDeclarado);

        public ResumenCajaDto ObtenerResumen(int cajaId)
        {
            var caja = _cajas.ObtenerExistente(cajaId);
            return CalcularResumen(caja, caja.EstaAbierta ? null : caja.MontoTotal);
        }

        private Caja ObtenerCajaAbierta() =>
            _cajas.ObtenerActiva() ?? throw new NoEncontradoException("No hay una caja abierta.");

        /// <summary>Diferencia = monto declarado - monto esperado (inicial + saldo neto de cada medio de pago).</summary>
        private ResumenCajaDto CalcularResumen(Caja caja, decimal? montoDeclarado)
        {
            var movimientos = _movimientos.ObtenerPorCaja(caja);
            var vigentes = movimientos.Where(m => !m.Anulado).ToList();
            var totalIngresos = vigentes.Where(m => m.Tipo == TipoMovimiento.Ingreso).Sum(m => m.Monto);
            var totalEgresos = vigentes.Where(m => m.Tipo == TipoMovimiento.Egreso).Sum(m => m.Monto);

            // Los totales por medio ya tienen revertidos los movimientos anulados.
            var totales = _totales.ObtenerPorCaja(caja);
            var montoEsperado = caja.MontoInicial + totales.Sum(t => t.Monto);
            decimal? diferencia = montoDeclarado.HasValue ? montoDeclarado.Value - montoEsperado : null;

            var totalesPorMedio = totales
                .OrderBy(t => t.MedioPago.Nombre)
                .Select(t => new TotalMedioPagoDto(t.MedioPago.Nombre, t.Monto))
                .ToList();

            return new ResumenCajaDto(
                CajaMapper.ADto(caja), caja.MontoInicial, totalIngresos, totalEgresos, montoEsperado,
                montoDeclarado, diferencia, totalesPorMedio, vigentes.Count, movimientos.Count - vigentes.Count);
        }
    }
}
