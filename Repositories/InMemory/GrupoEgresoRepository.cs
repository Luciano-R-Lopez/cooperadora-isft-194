using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class GrupoEgresoRepository : InMemoryRepository<GrupoEgreso>, IGrupoEgresoRepository
    {
        private static readonly Lazy<GrupoEgresoRepository> _instancia = new(() => new GrupoEgresoRepository());
        public static GrupoEgresoRepository Instancia => _instancia.Value;

        internal GrupoEgresoRepository() : base("El grupo de egreso seleccionado no existe.") { }
    }
}
