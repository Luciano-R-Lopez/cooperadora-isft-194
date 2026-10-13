namespace CooperadoraISFT94.Models
{
    /// <summary>
    /// Base de todas las entidades. El id lo asigna el repositorio al guardarlas
    /// (así, al migrar a MySQL, lo puede asignar la base); nadie más puede cambiarlo.
    /// </summary>
    public abstract class Entidad
    {
        public int Id { get; internal set; }
    }
}
