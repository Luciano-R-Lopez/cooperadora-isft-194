using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Repositories.MySql
{
    /// <summary>
    /// Lo que un repositorio le pide a MySQL para guardar sus cambios.
    /// Los repositorios siguen trabajando en memoria (así se mantienen las referencias entre objetos)
    /// y cada alta o modificación se escribe además en la base.
    /// </summary>
    public interface IPersistencia<T> where T : Entidad
    {
        /// <summary>Inserta la entidad. Devuelve el id que le asignó la base, o 0 si la tabla no tiene id propio.</summary>
        int Insertar(T entidad);

        void Actualizar(T entidad);

        void Eliminar(int id);
    }
}
