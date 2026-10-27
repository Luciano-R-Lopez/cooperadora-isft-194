using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class MotivoEgresoRepository : RepositorioBase<MotivoEgreso>
    {
        private static readonly MotivoEgresoRepository instancia = new MotivoEgresoRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal MotivoEgresoRepository() : base("El motivo de egreso seleccionado no existe.")
        {
        }

        public static MotivoEgresoRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
