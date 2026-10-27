using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
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
