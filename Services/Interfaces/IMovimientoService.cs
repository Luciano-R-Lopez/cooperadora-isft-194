using CooperadoraISFT94.DTOs.Movimientos;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IMovimientoService
    {
        /// <summary>Ingresos y egresos de una caja, filtrados, del más reciente al más antiguo.</summary>
        IReadOnlyList<MovimientoDto> Obtener(MovimientoFiltroDto filtro);
    }
}
