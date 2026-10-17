using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Repositories.MySql;

namespace CooperadoraISFT94.Repositories.InMemory
{
    /// <summary>
    /// Repositorio genérico respaldado por una lista en memoria.
    /// Con MySQL la lista actúa como caché: se llena al arrancar (<see cref="Cargar"/>) y, si tiene
    /// <see cref="Persistencia"/>, cada alta o cambio se escribe también en la base.
    /// Así los objetos conservan sus referencias entre sí y el resto de las capas no cambia.
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

        /// <summary>Si está asignada, los cambios también se guardan en MySQL. Sin ella funciona solo en memoria.</summary>
        public IPersistencia<T>? Persistencia { get; set; }

        /// <summary>Agrega una entidad leída de la base, sin volver a guardarla.</summary>
        internal void Cargar(T entidad)
        {
            ArgumentNullException.ThrowIfNull(entidad);

            if (entidad.Id <= 0)
                entidad.Id = _proximoId;
            if (entidad.Id >= _proximoId)
                _proximoId = entidad.Id + 1;

            Items.Add(entidad);
        }

        /// <summary>Devuelve una copia: quien la recorre no se ve afectado por altas posteriores.</summary>
        public IReadOnlyList<T> ObtenerTodos() => Items.ToList();

        public T? ObtenerPorId(int id) => Items.FirstOrDefault(e => e.Id == id);

        public T ObtenerExistente(int id) =>
            ObtenerPorId(id) ?? throw new NoEncontradoException(_mensajeNoEncontrado);

        public T Agregar(T entidad)
        {
            ArgumentNullException.ThrowIfNull(entidad);

            // Con base de datos, el id lo asigna MySQL; sin ella (o si la tabla no tiene id propio) se numera en memoria.
            var idPersistido = Persistencia?.Insertar(entidad) ?? 0;
            entidad.Id = idPersistido > 0 ? idPersistido : _proximoId;
            if (entidad.Id >= _proximoId)
                _proximoId = entidad.Id + 1;

            Items.Add(entidad);
            return entidad;
        }

        public void Actualizar(T entidad)
        {
            ArgumentNullException.ThrowIfNull(entidad);

            var indice = Items.FindIndex(e => e.Id == entidad.Id);
            if (indice < 0)
                throw new NoEncontradoException(_mensajeNoEncontrado);

            Persistencia?.Actualizar(entidad);
            Items[indice] = entidad;
        }

        public void Eliminar(int id)
        {
            var existente = ObtenerExistente(id);

            Persistencia?.Eliminar(id);
            Items.Remove(existente);
        }
    }
}
