namespace CooperadoraISFT94.Models
{
    /// <summary>A quién corresponde un gasto (estudiantes, administración, otros). Es independiente del motivo.</summary>
    public class GrupoEgreso : ItemCatalogo
    {
        public GrupoEgreso(string nombre) : base(nombre, "grupo de egreso") { }
    }
}
