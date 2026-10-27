namespace CooperadoraISFT94.Domain
{
    // A quién corresponde un gasto (estudiantes, administración, otros). Es independiente del motivo.
    public class GrupoEgreso : ItemCatalogo
    {
        public GrupoEgreso(int id, string nombre) : base(id, nombre, "grupo de egreso")
        {
        }
    }
}
