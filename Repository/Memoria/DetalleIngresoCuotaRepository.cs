using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class DetalleIngresoCuotaRepository : RepositorioBase<DetalleIngresoCuota>
    {
        private static readonly DetalleIngresoCuotaRepository instancia = new DetalleIngresoCuotaRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal DetalleIngresoCuotaRepository() : base("El detalle del cobro seleccionado no existe.")
        {
        }

        public static DetalleIngresoCuotaRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
