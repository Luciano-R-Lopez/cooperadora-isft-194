using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using MySqlConnector;

namespace CooperadoraISFT94.Repository.MySql
{
    // Los datos se cargan de la base al iniciar (CargadorMySql) y se consultan en memoria; cada alta,
    // modificación o baja se escribe primero en la base y, si salió bien, en memoria.
    // Por eso la aplicación tiene que estar abierta en una sola computadora a la vez.
    internal class RepositorioMySql<T> : IRepository<T> where T : class, IEntidad
    {
        private const string MensajeSinBase =
            "No se pudo guardar en la base de datos. Revise que MySQL esté prendido en XAMPP. Detalle: ";

        private readonly IRepository<T> memoria;
        private readonly IPersistencia<T> persistencia;

        public RepositorioMySql(IRepository<T> memoria, IPersistencia<T> persistencia)
        {
            this.memoria = memoria;
            this.persistencia = persistencia;
        }

        public int ObtenerProximoId()
        {
            return memoria.ObtenerProximoId();
        }

        public void Agregar(T elemento)
        {
            Guardar(() => persistencia.Insertar(elemento));
            memoria.Agregar(elemento);
        }

        public void Actualizar(T elemento)
        {
            Guardar(() => persistencia.Actualizar(elemento));
            memoria.Actualizar(elemento);
        }

        public IReadOnlyList<T> ObtenerTodos()
        {
            return memoria.ObtenerTodos();
        }

        public T ObtenerExistente(int id)
        {
            return memoria.ObtenerExistente(id);
        }

        public void Eliminar(int id)
        {
            Guardar(() => persistencia.Eliminar(id));
            memoria.Eliminar(id);
        }

        // Un problema con la base (MySQL apagado, sin conexión) se muestra como aviso y no como error inesperado.
        private static void Guardar(Action escribir)
        {
            try
            {
                escribir();
            }
            catch (MySqlException ex)
            {
                throw new NegocioException(MensajeSinBase + ex.Message);
            }
        }
    }
}
