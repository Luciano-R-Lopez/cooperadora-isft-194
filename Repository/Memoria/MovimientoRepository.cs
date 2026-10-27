using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class MovimientoRepository : InMemoryRepository<Movimiento>, IMovimientoRepository
    {
        private static readonly Lazy<MovimientoRepository> _instancia = new(() => new MovimientoRepository());
        public static MovimientoRepository Instancia => _instancia.Value;

        internal MovimientoRepository() : base("El movimiento seleccionado no existe.") { }

        public IReadOnlyList<Movimiento> ObtenerPorCaja(Caja caja) =>
            Items.Where(m => ReferenceEquals(m.Caja, caja)).ToList();
    }
}
