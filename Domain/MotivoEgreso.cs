namespace CooperadoraISFT94.Domain
{
    /// <summary>Razón por la que sale dinero de la caja.</summary>
    public class MotivoEgreso : ItemCatalogo
    {
        public MotivoEgreso(string nombre) : base(nombre, "motivo de egreso") { }
    }
}
