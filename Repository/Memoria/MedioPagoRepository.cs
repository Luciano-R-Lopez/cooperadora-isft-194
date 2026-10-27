using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class MedioPagoRepository : InMemoryRepository<MedioPago>, IMedioPagoRepository
    {
        private static readonly Lazy<MedioPagoRepository> _instancia = new(() => new MedioPagoRepository());
        public static MedioPagoRepository Instancia => _instancia.Value;

        internal MedioPagoRepository() : base("El medio de pago seleccionado no existe.") { }
    }
}
