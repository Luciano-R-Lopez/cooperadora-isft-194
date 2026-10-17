using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Movimientos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class MovimientoService : IMovimientoService
    {
        private const string SinDetalle = "-";

        private static readonly Lazy<MovimientoService> _instancia = new(() =>
            new MovimientoService(MovimientoRepository.Instancia, DetalleIngresoCuotaRepository.Instancia,
                                  MedioPagoRepository.Instancia, CajaRepository.Instancia));
        public static MovimientoService Instancia => _instancia.Value;

        private readonly IMovimientoRepository _movimientos;
        private readonly IDetalleIngresoCuotaRepository _detalles;
        private readonly IMedioPagoRepository _medios;
        private readonly ICajaRepository _cajas;

        private MovimientoService(IMovimientoRepository movimientos, IDetalleIngresoCuotaRepository detalles,
                                  IMedioPagoRepository medios, ICajaRepository cajas)
        {
            _movimientos = movimientos;
            _detalles = detalles;
            _medios = medios;
            _cajas = cajas;
        }

        public IReadOnlyList<MovimientoDto> Obtener(MovimientoFiltroDto filtro)
        {
            ArgumentNullException.ThrowIfNull(filtro);

            var caja = _cajas.ObtenerExistente(filtro.CajaId);
            if (filtro.Tipo is TipoMovimiento tipoElegido && !Enum.IsDefined(tipoElegido))
                throw new ReglaNegocioException("El tipo de movimiento no es válido.");
            if (filtro.MedioPagoId is int medioId)
                _medios.ObtenerExistente(medioId);

            IEnumerable<Movimiento> movimientos = _movimientos.ObtenerPorCaja(caja);

            if (filtro.Tipo is TipoMovimiento tipo)
                movimientos = movimientos.Where(m => m.Tipo == tipo);

            if (filtro.MedioPagoId is int medioPagoId)
                movimientos = movimientos.Where(m => m.MedioPago.Id == medioPagoId);

            // Cuotas que pagó cada ingreso, indexadas por id de ingreso. Un ingreso y un egreso pueden compartir id
            // (son tablas distintas en la base), por eso solo se buscan detalles para los ingresos.
            var cuotasPorMovimiento = _detalles.ObtenerTodos().ToLookup(d => d.Ingreso.Id);

            IEnumerable<MovimientoDto> resultado = movimientos.Select(m => new MovimientoDto(
                m.Tipo, m.Fecha, m.Clasificacion, DescribirDetalle(m.Tipo == TipoMovimiento.Ingreso
                    ? cuotasPorMovimiento[m.Id]
                    : Enumerable.Empty<DetalleIngresoCuota>()),
                m.MedioPago.Nombre, m.Monto, m.Usuario.NombreCompleto));

            if (!string.IsNullOrWhiteSpace(filtro.Texto))
            {
                var texto = filtro.Texto.Trim();
                resultado = resultado.Where(m =>
                    m.Detalle.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    m.Clasificacion.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    m.Usuario.Contains(texto, StringComparison.OrdinalIgnoreCase));
            }

            return resultado.OrderByDescending(m => m.Fecha).ToList();
        }

        private static string DescribirDetalle(IEnumerable<DetalleIngresoCuota> detalles)
        {
            var cuotas = detalles
                .Select(d => $"Cuota {d.Cuota.Anio} - {d.Cuota.Socio.ObtenerDescripcion()}")
                .ToList();

            return cuotas.Count == 0 ? SinDetalle : string.Join("; ", cuotas);
        }
    }
}
