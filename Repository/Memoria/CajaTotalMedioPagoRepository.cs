using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class CajaTotalMedioPagoRepository : RepositorioBase<CajaTotalMedioPago>
    {
        private static readonly CajaTotalMedioPagoRepository instancia = new CajaTotalMedioPagoRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal CajaTotalMedioPagoRepository() : base("El total por medio de pago seleccionado no existe.")
        {
        }

        public static CajaTotalMedioPagoRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
