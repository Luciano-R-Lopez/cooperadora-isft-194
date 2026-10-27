using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface ICajaRepository : IRepository<Caja>
    {
        /// <summary>La caja abierta actualmente, o null si no hay ninguna.</summary>
        Caja? ObtenerActiva();
    }
}
