using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
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
