using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class MovimientoRepository : RepositorioBase<Movimiento>
    {
        private static readonly MovimientoRepository instancia = new MovimientoRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal MovimientoRepository() : base("El movimiento seleccionado no existe.")
        {
        }

        public static MovimientoRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
