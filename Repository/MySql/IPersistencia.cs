using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Repository.MySql
{
    // Cómo se escribe una entidad en sus tablas. La entidad ya tiene su id: se guarda con ese mismo id.
    internal interface IPersistencia<T> where T : class, IEntidad
    {
        void Insertar(T elemento);
        void Actualizar(T elemento);
        void Eliminar(int id);
    }
}
