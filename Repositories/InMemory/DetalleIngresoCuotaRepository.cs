using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class DetalleIngresoCuotaRepository : InMemoryRepository<DetalleIngresoCuota>, IDetalleIngresoCuotaRepository
    {
        private static readonly Lazy<DetalleIngresoCuotaRepository> _instancia = new(() => new DetalleIngresoCuotaRepository());
        public static DetalleIngresoCuotaRepository Instancia => _instancia.Value;

        internal DetalleIngresoCuotaRepository() : base("El detalle de ingreso seleccionado no existe.") { }

        public IReadOnlyList<DetalleIngresoCuota> ObtenerPorIngreso(Ingreso ingreso) =>
            Items.Where(d => ReferenceEquals(d.Ingreso, ingreso)).ToList();
    }
}
