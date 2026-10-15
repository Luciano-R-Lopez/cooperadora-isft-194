using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    /// <summary>Guarda ingresos y egresos juntos: un movimiento es un movimiento sin importar su tipo.</summary>
    public interface IMovimientoRepository : IRepository<Movimiento>
    {
        /// <summary>Movimientos que se aplicaron a esa caja.</summary>
        IReadOnlyList<Movimiento> ObtenerPorCaja(Caja caja);
    }
}
