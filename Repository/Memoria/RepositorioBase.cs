using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.Memoria
{
    public abstract class RepositorioBase<T> : IRepository<T> where T : class, IEntidad
    {
        protected readonly List<T> elementos = new List<T>();
        private readonly string mensajeNoEncontrado;
        private int proximoId = 1;

        protected RepositorioBase(string mensajeNoEncontrado)
        {
            this.mensajeNoEncontrado = mensajeNoEncontrado;
        }

        public int ObtenerProximoId()
        {
            int idAsignado = proximoId;
            proximoId++;
            return idAsignado;
        }

        public void Agregar(T elemento)
        {
            if (elementos.Any(existente => existente.Id == elemento.Id))
            {
                throw new InvalidOperationException("Ya hay un elemento guardado con el id " + elemento.Id + ".");
            }
            elementos.Add(elemento);
            // Los datos que se cargan de la base ya traen su id: los nuevos siguen después del mayor.
            proximoId = Math.Max(proximoId, elemento.Id + 1);
        }

        public void Actualizar(T elemento)
        {
            int indice = elementos.FindIndex(existente => existente.Id == elemento.Id);
            if (indice < 0)
            {
                throw new NoEncontradoException(mensajeNoEncontrado);
            }
            elementos[indice] = elemento;
        }

        // Devuelve una copia: quien la recorre no se ve afectado si después se agregan elementos.
        public IReadOnlyList<T> ObtenerTodos()
        {
            return elementos.ToList().AsReadOnly();
        }

        public T ObtenerExistente(int id)
        {
            T? elemento = elementos.Find(e => e.Id == id);
            if (elemento == null)
            {
                throw new NoEncontradoException(mensajeNoEncontrado);
            }
            return elemento;
        }

        public void Eliminar(int id)
        {
            if (elementos.RemoveAll(elemento => elemento.Id == id) == 0)
            {
                throw new NoEncontradoException(mensajeNoEncontrado);
            }
        }
    }
}
