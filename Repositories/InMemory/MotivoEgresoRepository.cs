using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class MotivoEgresoRepository : InMemoryRepository<MotivoEgreso>, IMotivoEgresoRepository
    {
        private static readonly Lazy<MotivoEgresoRepository> _instancia = new(() => new MotivoEgresoRepository());
        public static MotivoEgresoRepository Instancia => _instancia.Value;

        internal MotivoEgresoRepository() : base("El motivo de egreso seleccionado no existe.") { }
    }
}
