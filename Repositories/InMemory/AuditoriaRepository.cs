using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class AuditoriaRepository : InMemoryRepository<Auditoria>, IAuditoriaRepository
    {
        private static readonly Lazy<AuditoriaRepository> _instancia = new(() => new AuditoriaRepository());
        public static AuditoriaRepository Instancia => _instancia.Value;

        internal AuditoriaRepository() : base("El registro de auditoría seleccionado no existe.") { }
    }
}
