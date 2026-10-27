using CooperadoraISFT94.DTOs.Movimientos;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IMovimientoService
    {
        /// <summary>Ingresos y egresos de una caja (incluidos los anulados), filtrados, del más reciente al más antiguo.</summary>
        IReadOnlyList<MovimientoDto> Obtener(MovimientoFiltroDto filtro);

        /// <summary>
        /// Anula un movimiento de la caja abierta revirtiendo su efecto. Si era el cobro de cuotas,
        /// esas cuotas vuelven a quedar pendientes.
        /// </summary>
        MovimientoDto Anular(int movimientoId, string motivo, int usuarioId);

        /// <summary>
        /// Egresos vigentes (sin los anulados) de todas las cajas entre dos fechas, ambas incluidas,
        /// ordenados por grupo y fecha. Para exportar los gastos de un período.
        /// </summary>
        IReadOnlyList<MovimientoDto> ObtenerEgresos(DateTime desde, DateTime hasta);
    }
}
