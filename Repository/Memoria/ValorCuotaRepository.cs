using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class ValorCuotaRepository : RepositorioBase<ValorCuota>
    {
        private static readonly ValorCuotaRepository instancia = new ValorCuotaRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal ValorCuotaRepository() : base("El valor de cuota seleccionado no existe.")
        {
        }

        public static ValorCuotaRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
