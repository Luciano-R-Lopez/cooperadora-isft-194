using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository
{
    public interface IRepository<T> where T : class, IEntidad
    {
        int ObtenerProximoId();
        void Agregar(T elemento);
        void Actualizar(T elemento);
        IReadOnlyList<T> ObtenerTodos();
        T ObtenerExistente(int id);
        void Eliminar(int id);
    }
}
