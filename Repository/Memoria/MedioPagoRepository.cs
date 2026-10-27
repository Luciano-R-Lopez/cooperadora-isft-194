using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class MedioPagoRepository : RepositorioBase<MedioPago>
    {
        private static readonly MedioPagoRepository instancia = new MedioPagoRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal MedioPagoRepository() : base("El medio de pago seleccionado no existe.")
        {
        }

        public static MedioPagoRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
