using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class MedioPagoRepository : InMemoryRepository<MedioPago>, IMedioPagoRepository
    {
        private static readonly Lazy<MedioPagoRepository> _instancia = new(() => new MedioPagoRepository());
        public static MedioPagoRepository Instancia => _instancia.Value;

        internal MedioPagoRepository() : base("El medio de pago seleccionado no existe.") { }
    }
}
