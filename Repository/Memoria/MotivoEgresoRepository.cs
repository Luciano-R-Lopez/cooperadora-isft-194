using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class MotivoEgresoRepository : InMemoryRepository<MotivoEgreso>, IMotivoEgresoRepository
    {
        private static readonly Lazy<MotivoEgresoRepository> _instancia = new(() => new MotivoEgresoRepository());
        public static MotivoEgresoRepository Instancia => _instancia.Value;

        internal MotivoEgresoRepository() : base("El motivo de egreso seleccionado no existe.") { }
    }
}
