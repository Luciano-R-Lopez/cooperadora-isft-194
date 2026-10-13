using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class SocioRepository : InMemoryRepository<Socio>, ISocioRepository
    {
        private static readonly Lazy<SocioRepository> _instancia = new(() => new SocioRepository());
        public static SocioRepository Instancia => _instancia.Value;

        private SocioRepository() : base("El socio seleccionado no existe.") { }

        public Socio? ObtenerPorDni(string dni) =>
            Items.FirstOrDefault(s => s.Dni == dni);

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
