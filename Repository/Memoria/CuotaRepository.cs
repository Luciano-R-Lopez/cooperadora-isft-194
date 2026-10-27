using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class CuotaRepository : InMemoryRepository<Cuota>, ICuotaRepository
    {
        private static readonly Lazy<CuotaRepository> _instancia = new(() => new CuotaRepository());
        public static CuotaRepository Instancia => _instancia.Value;

        internal CuotaRepository() : base("La cuota seleccionada no existe.") { }

        public IReadOnlyList<Cuota> ObtenerPorSocio(Socio socio) =>
            Items.Where(c => ReferenceEquals(c.Socio, socio))
                 .OrderByDescending(c => c.Anio)
                 .ToList();

        public bool ExisteParaAnio(Socio socio, int anio) =>
            Items.Any(c => ReferenceEquals(c.Socio, socio) && c.Anio == anio);

        public Cuota? ObtenerUltimaDelAnio(int anio) =>
            Items.Where(c => c.Anio == anio).MaxBy(c => c.Id);

        public IReadOnlyList<Cuota> ObtenerPendientes(int? anio) =>
            Items.Where(c => !c.EstaPagada && (anio is null || c.Anio == anio))
                 .OrderBy(c => c.Anio)
                 .ThenBy(c => c.Socio.Apellido)
                 .ThenBy(c => c.Socio.Nombre)
                 .ToList();
    }
}
