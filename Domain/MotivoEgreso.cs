namespace CooperadoraISFT94.Domain
{
    // Razón por la que sale dinero de la caja.
    public class MotivoEgreso : ItemCatalogo
    {
        public MotivoEgreso(int id, string nombre) : base(id, nombre, "motivo de egreso")
        {
        }
    }
}
