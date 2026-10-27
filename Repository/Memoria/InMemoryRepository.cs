using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Repository.Memoria
{
    /// <summary>
    /// Repositorio genérico respaldado por una lista en memoria.
    /// Cuando se pase a MySQL se reemplazan estas clases por implementaciones de las mismas
    /// interfaces y el resto de las capas no cambia.
    /// </summary>
    public abstract class InMemoryRepository<T> : IRepository<T> where T : Entidad
    {
        protected readonly List<T> Items = new();
        private readonly string _mensajeNoEncontrado;
        private int _proximoId = 1;

        /// <param name="mensajeNoEncontrado">Texto del error cuando no existe el elemento buscado (ej.: "El socio seleccionado no existe.").</param>
        protected InMemoryRepository(string mensajeNoEncontrado)
        {
            _mensajeNoEncontrado = mensajeNoEncontrado;
        }

        /// <summary>Devuelve una copia: quien la recorre no se ve afectado por altas posteriores.</summary>
        public IReadOnlyList<T> ObtenerTodos() => Items.ToList();

        public T? ObtenerPorId(int id) => Items.FirstOrDefault(e => e.Id == id);

        public T ObtenerExistente(int id) =>
            ObtenerPorId(id) ?? throw new NoEncontradoException(_mensajeNoEncontrado);

        public T Agregar(T entidad)
        {
            ArgumentNullException.ThrowIfNull(entidad);

            entidad.Id = _proximoId++;
            Items.Add(entidad);
            return entidad;
        }

        public void Actualizar(T entidad)
        {
            ArgumentNullException.ThrowIfNull(entidad);

            var indice = Items.FindIndex(e => e.Id == entidad.Id);
            if (indice < 0)
                throw new NoEncontradoException(_mensajeNoEncontrado);
            Items[indice] = entidad;
        }

        public void Eliminar(int id)
        {
            if (Items.RemoveAll(e => e.Id == id) == 0)
                throw new NoEncontradoException(_mensajeNoEncontrado);
        }
    }
}
