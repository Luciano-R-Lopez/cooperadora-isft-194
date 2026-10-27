using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    public sealed class SocioRepository : InMemoryRepository<Socio>, ISocioRepository
    {
        private static readonly Lazy<SocioRepository> _instancia = new(() => new SocioRepository());
        public static SocioRepository Instancia => _instancia.Value;

        internal SocioRepository() : base("El socio seleccionado no existe.") { }

        public Socio? ObtenerPorDni(string dni) =>
            Items.FirstOrDefault(s => s.Dni == dni);

        public IReadOnlyList<Socio> ObtenerResponsables() =>
            Items.Where(s => s.EsResponsable && s.Activo)
                 .OrderBy(s => s.Apellido)
                 .ThenBy(s => s.Nombre)
                 .ToList();

        public IReadOnlyList<Socio> ObtenerACargo(Socio responsable) =>
            Items.Where(s => ReferenceEquals(s.Responsable, responsable))
                 .OrderBy(s => s.Apellido)
                 .ThenBy(s => s.Nombre)
                 .ToList();

        public IReadOnlyList<Socio> Buscar(string? filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return Items.OrderBy(s => s.Apellido).ThenBy(s => s.Nombre).ToList();

            var texto = filtro.Trim();

            return Items
                .Where(s => s.Dni.Contains(texto, StringComparison.OrdinalIgnoreCase)
                         || $"{s.Nombre} {s.Apellido}".Contains(texto, StringComparison.OrdinalIgnoreCase)
                         || $"{s.Apellido} {s.Nombre}".Contains(texto, StringComparison.OrdinalIgnoreCase))
                .OrderBy(s => s.Apellido)
                .ThenBy(s => s.Nombre)
                .ToList();
        }
    }
}
