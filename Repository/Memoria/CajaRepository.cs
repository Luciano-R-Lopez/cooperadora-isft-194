using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class CajaRepository : RepositorioBase<Caja>
    {
        private static readonly CajaRepository instancia = new CajaRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal CajaRepository() : base("La caja seleccionada no existe.")
        {
        }

        public static CajaRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
