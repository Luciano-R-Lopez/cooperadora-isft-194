using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Service
{
    public sealed class MovimientoService : IMovimientoService
    {
        private const string SinDetalle = "-";

        private static readonly Lazy<MovimientoService> _instancia = new(() =>
            new MovimientoService(MovimientoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                                  MedioPagoRepository.Instancia, CajaRepository.Instancia, UsuarioRepository.Instancia,
                                  CuotaRepository.Instancia, GrupoEgresoRepository.Instancia, MovimientoRegistrador.Instancia,
                                  AuditoriaService.Instancia));
        public static MovimientoService Instancia => _instancia.Value;

        private readonly IMovimientoRepository _movimientos;
        private readonly IDetalleIngresoCuotaRepository _detalles;
        private readonly IMedioPagoRepository _medios;
        private readonly IGrupoEgresoRepository _grupos;
        private readonly ICajaRepository _cajas;
        private readonly IUsuarioRepository _usuarios;
        private readonly ICuotaRepository _cuotas;
        private readonly IMovimientoRegistrador _registrador;
        private readonly IAuditoriaService _auditoria;

        internal MovimientoService(IMovimientoRepository movimientos, IDetalleIngresoCuotaRepository detalles,
                                   IMedioPagoRepository medios, ICajaRepository cajas, IUsuarioRepository usuarios,
                                   ICuotaRepository cuotas, IGrupoEgresoRepository grupos, IMovimientoRegistrador registrador,
                                   IAuditoriaService auditoria)
        {
            _movimientos = movimientos;
            _detalles = detalles;
            _medios = medios;
            _cajas = cajas;
            _usuarios = usuarios;
            _cuotas = cuotas;
            _grupos = grupos;
            _registrador = registrador;
            _auditoria = auditoria;
        }

        public IReadOnlyList<MovimientoDto> Obtener(MovimientoFiltroDto filtro)
        {
            ArgumentNullException.ThrowIfNull(filtro);

            var caja = _cajas.ObtenerExistente(filtro.CajaId);
            if (filtro.Tipo is TipoMovimiento tipoElegido && !Enum.IsDefined(tipoElegido))
                throw new ReglaNegocioException("El tipo de movimiento no es válido.");
            if (filtro.MedioPagoId is int medioId)
                _medios.ObtenerExistente(medioId);
            if (filtro.GrupoEgresoId is int grupoId)
                _grupos.ObtenerExistente(grupoId);

            IEnumerable<Movimiento> movimientos = _movimientos.ObtenerPorCaja(caja);

            if (filtro.Tipo is TipoMovimiento tipo)
                movimientos = movimientos.Where(m => m.Tipo == tipo);

            if (filtro.MedioPagoId is int medioPagoId)
                movimientos = movimientos.Where(m => m.MedioPago.Id == medioPagoId);

            if (filtro.GrupoEgresoId is int grupoEgresoId)
                movimientos = movimientos.Where(m => m is Egreso e && e.Grupo.Id == grupoEgresoId);

            // Cuotas que pagó cada ingreso, indexadas por id de movimiento (los egresos simplemente no tienen).
            var cuotasPorMovimiento = _detalles.ObtenerTodos().ToLookup(d => d.Ingreso.Id);

            IEnumerable<MovimientoDto> resultado = movimientos.Select(m => ADto(m, cuotasPorMovimiento[m.Id]));

            if (!string.IsNullOrWhiteSpace(filtro.Texto))
            {
                var texto = filtro.Texto.Trim();
                resultado = resultado.Where(m =>
                    m.Detalle.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    m.Clasificacion.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    m.Grupo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    m.Usuario.Contains(texto, StringComparison.OrdinalIgnoreCase));
            }

            return resultado.OrderByDescending(m => m.Fecha).ThenByDescending(m => m.Id).ToList();
        }

        public MovimientoDto Anular(int movimientoId, string motivo, int usuarioId)
        {
            var movimiento = _movimientos.ObtenerExistente(movimientoId);
            var usuario = _usuarios.ObtenerExistente(usuarioId);
            var detalles = movimiento is Ingreso ingreso ? _detalles.ObtenerPorIngreso(ingreso) : Array.Empty<DetalleIngresoCuota>();

            // Primero la caja: si la anulación falla (por ejemplo, saldo insuficiente), las cuotas quedan como estaban.
            _registrador.Anular(movimiento, usuario, motivo);

            foreach (var cuota in detalles.Select(d => d.Cuota).Where(c => c.EstaPagada))
            {
                cuota.RevertirPago();
                _cuotas.Actualizar(cuota);
                _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Cuota), cuota.Id,
                                     "Estado: Pagada → Pendiente (se anuló el ingreso)");
            }

            return ADto(movimiento, detalles);
        }

        public IReadOnlyList<MovimientoDto> ObtenerEgresos(DateTime desde, DateTime hasta)
        {
            if (desde.Date > hasta.Date)
                throw new ReglaNegocioException("La fecha \"desde\" no puede ser posterior a la fecha \"hasta\".");

            return _movimientos.ObtenerTodos()
                               .OfType<Egreso>()
                               .Where(e => !e.Anulado && e.Fecha.Date >= desde.Date && e.Fecha.Date <= hasta.Date)
                               .OrderBy(e => e.Grupo.Nombre)
                               .ThenBy(e => e.Fecha)
                               .Select(e => ADto(e, Enumerable.Empty<DetalleIngresoCuota>()))
                               .ToList();
        }

        private static MovimientoDto ADto(Movimiento m, IEnumerable<DetalleIngresoCuota> detalles) =>
            new(m.Id, m.Tipo, m.Fecha, m.Clasificacion, (m as Egreso)?.Grupo.Nombre ?? SinDetalle, DescribirDetalle(detalles),
                m.MedioPago.Nombre, m.Monto, m.Usuario.NombreCompleto, m.Anulado, m.MotivoAnulacion);

        private static string DescribirDetalle(IEnumerable<DetalleIngresoCuota> detalles)
        {
            var cuotas = detalles.Select(DescribirCuota).ToList();
            return cuotas.Count == 0 ? SinDetalle : string.Join("; ", cuotas);
        }

        /// <summary>"Cuota 2026 - Pérez, Sofía (40111222)", y quién pagó si fue su responsable.</summary>
        private static string DescribirCuota(DetalleIngresoCuota d) =>
            d.PagadaPor is Socio responsable
                ? $"Cuota {d.Cuota.Anio} - {d.Cuota.Socio.ObtenerDescripcion()} (pagó {responsable.Apellido}, {responsable.Nombre})"
                : $"Cuota {d.Cuota.Anio} - {d.Cuota.Socio.ObtenerDescripcion()}";
    }
}
