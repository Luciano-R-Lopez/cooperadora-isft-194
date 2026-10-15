using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;

namespace CooperadoraISFT94.Repositories.InMemory
{
    public sealed class UsuarioRepository : InMemoryRepository<Usuario>, IUsuarioRepository
    {
        private static readonly Lazy<UsuarioRepository> _instancia = new(() => new UsuarioRepository());
        public static UsuarioRepository Instancia => _instancia.Value;

        internal UsuarioRepository() : base("El usuario seleccionado no existe.") { }

        public Usuario? ObtenerPorNombreUsuario(string nombreUsuario) =>
            Items.FirstOrDefault(u => string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase));
    }
}
