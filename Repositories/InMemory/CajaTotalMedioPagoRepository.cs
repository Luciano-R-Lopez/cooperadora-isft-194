using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class CajaTotalMedioPagoRepository : InMemoryRepository<CajaTotalMedioPago>, ICajaTotalMedioPagoRepository
    {
        private static readonly Lazy<CajaTotalMedioPagoRepository> _instancia = new(() => new CajaTotalMedioPagoRepository());
        public static CajaTotalMedioPagoRepository Instancia => _instancia.Value;

        private CajaTotalMedioPagoRepository() : base("El total por medio de pago seleccionado no existe.") { }

        public IReadOnlyList<CajaTotalMedioPago> ObtenerPorCaja(Caja caja) =>
            Items.Where(t => ReferenceEquals(t.Caja, caja)).ToList();

        public CajaTotalMedioPago? Obtener(Caja caja, MedioPago medioPago) =>
            Items.FirstOrDefault(t => ReferenceEquals(t.Caja, caja) && ReferenceEquals(t.MedioPago, medioPago));
    }
}
