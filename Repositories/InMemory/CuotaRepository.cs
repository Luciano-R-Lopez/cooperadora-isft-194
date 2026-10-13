using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class CuotaRepository : InMemoryRepository<Cuota>, ICuotaRepository
    {
        private static readonly Lazy<CuotaRepository> _instancia = new(() => new CuotaRepository());
        public static CuotaRepository Instancia => _instancia.Value;

        private CuotaRepository() : base("La cuota seleccionada no existe.") { }

        public IReadOnlyList<Cuota> ObtenerPorSocio(Socio socio) =>
            Items.Where(c => ReferenceEquals(c.Socio, socio))
                 .OrderByDescending(c => c.Anio)
                 .ToList();

        public bool ExisteParaAnio(Socio socio, int anio) =>
            Items.Any(c => ReferenceEquals(c.Socio, socio) && c.Anio == anio);
    }
}
