using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class GrupoEgresoRepository : RepositorioBase<GrupoEgreso>
    {
        private static readonly GrupoEgresoRepository instancia = new GrupoEgresoRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal GrupoEgresoRepository() : base("El grupo de egreso seleccionado no existe.")
        {
        }

        public static GrupoEgresoRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
