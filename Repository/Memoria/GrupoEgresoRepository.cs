using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class GrupoEgresoRepository : InMemoryRepository<GrupoEgreso>, IGrupoEgresoRepository
    {
        private static readonly Lazy<GrupoEgresoRepository> _instancia = new(() => new GrupoEgresoRepository());
        public static GrupoEgresoRepository Instancia => _instancia.Value;

        internal GrupoEgresoRepository() : base("El grupo de egreso seleccionado no existe.") { }
    }
}
