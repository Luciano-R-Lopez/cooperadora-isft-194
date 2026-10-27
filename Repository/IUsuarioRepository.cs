using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario? ObtenerPorNombreUsuario(string nombreUsuario);
    }
}
