using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class SocioRepository : RepositorioBase<Socio>
    {
        private static readonly SocioRepository instancia = new SocioRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal SocioRepository() : base("El socio seleccionado no existe.")
        {
        }

        public static SocioRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
