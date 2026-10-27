using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface IRepository<T> where T : Entidad
    {
        IReadOnlyList<T> ObtenerTodos();

        /// <summary>Devuelve null si no existe.</summary>
        T? ObtenerPorId(int id);

        /// <summary>Igual que ObtenerPorId pero lanza NoEncontradoException en lugar de devolver null.</summary>
        T ObtenerExistente(int id);

        T Agregar(T entidad);

        /// <summary>Lanza NoEncontradoException si la entidad no está guardada.</summary>
        void Actualizar(T entidad);

        /// <summary>Lanza NoEncontradoException si no existe.</summary>
        void Eliminar(int id);
    }
}
