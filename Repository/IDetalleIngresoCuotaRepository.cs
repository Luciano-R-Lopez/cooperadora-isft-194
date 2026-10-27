using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface IDetalleIngresoCuotaRepository : IRepository<DetalleIngresoCuota>
    {
        /// <summary>Cuotas que pagó el ingreso indicado (vacío si no es un ingreso de cuota social).</summary>
        IReadOnlyList<DetalleIngresoCuota> ObtenerPorIngreso(Ingreso ingreso);
    }
}
