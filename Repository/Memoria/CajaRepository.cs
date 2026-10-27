using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class CajaRepository : InMemoryRepository<Caja>, ICajaRepository
    {
        private static readonly Lazy<CajaRepository> _instancia = new(() => new CajaRepository());
        public static CajaRepository Instancia => _instancia.Value;

        internal CajaRepository() : base("La caja seleccionada no existe.") { }

        public Caja? ObtenerActiva() => Items.FirstOrDefault(c => c.EstaAbierta);
    }
}
