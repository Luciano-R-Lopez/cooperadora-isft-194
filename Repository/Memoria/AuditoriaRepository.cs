using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public class AuditoriaRepository : RepositorioBase<Auditoria>
    {
        private static readonly AuditoriaRepository instancia = new AuditoriaRepository();

        // Interno (y no privado) solo para que los tests puedan trabajar con un repositorio vacío.
        internal AuditoriaRepository() : base("El registro de auditoría seleccionado no existe.")
        {
        }

        public static AuditoriaRepository ObtenerInstancia()
        {
            return instancia;
        }
    }
}
