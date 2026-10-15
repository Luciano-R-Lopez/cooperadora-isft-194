using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.Interfaces
{
    public interface IDetalleIngresoCuotaRepository : IRepository<DetalleIngresoCuota>
    {
        /// <summary>Cuotas que pagó el ingreso indicado (vacío si no es un ingreso de cuota social).</summary>
        IReadOnlyList<DetalleIngresoCuota> ObtenerPorIngreso(Ingreso ingreso);
    }
}
