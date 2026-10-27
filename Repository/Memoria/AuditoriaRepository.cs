using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class AuditoriaRepository : InMemoryRepository<Auditoria>, IAuditoriaRepository
    {
        private static readonly Lazy<AuditoriaRepository> _instancia = new(() => new AuditoriaRepository());
        public static AuditoriaRepository Instancia => _instancia.Value;

        internal AuditoriaRepository() : base("El registro de auditoría seleccionado no existe.") { }
    }
}
