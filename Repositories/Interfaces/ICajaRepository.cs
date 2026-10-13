using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    public interface ICajaRepository : IRepository<Caja>
    {
        /// <summary>La caja abierta actualmente, o null si no hay ninguna.</summary>
        Caja? ObtenerActiva();
    }
}
