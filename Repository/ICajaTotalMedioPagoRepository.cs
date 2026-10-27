using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface ICajaTotalMedioPagoRepository : IRepository<CajaTotalMedioPago>
    {
        IReadOnlyList<CajaTotalMedioPago> ObtenerPorCaja(Caja caja);

        /// <summary>El total de la caja para ese medio, o null si todavía no tuvo movimientos con él.</summary>
        CajaTotalMedioPago? Obtener(Caja caja, MedioPago medioPago);
    }
}
