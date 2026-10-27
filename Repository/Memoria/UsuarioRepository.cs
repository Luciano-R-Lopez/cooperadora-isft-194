using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class UsuarioRepository : RepositorioBase<Usuario>
    {
        private static readonly UsuarioRepository instancia = new UsuarioRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal UsuarioRepository() : base("El usuario seleccionado no existe.")
        {
        }

        public static UsuarioRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
