using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class CuotaRepository : RepositorioBase<Cuota>
    {
        private static readonly CuotaRepository instancia = new CuotaRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal CuotaRepository() : base("La cuota seleccionada no existe.")
        {
        }

        public static CuotaRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
