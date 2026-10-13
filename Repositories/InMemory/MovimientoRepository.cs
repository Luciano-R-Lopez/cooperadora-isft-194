using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class MovimientoRepository : InMemoryRepository<Movimiento>, IMovimientoRepository
    {
        private static readonly Lazy<MovimientoRepository> _instancia = new(() => new MovimientoRepository());
        public static MovimientoRepository Instancia => _instancia.Value;

        private MovimientoRepository() : base("El movimiento seleccionado no existe.") { }

        public IReadOnlyList<Movimiento> ObtenerPorCaja(Caja caja) =>
            Items.Where(m => caja.Abarca(m.Fecha)).ToList();
    }
}
