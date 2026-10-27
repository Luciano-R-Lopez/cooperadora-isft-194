namespace CooperadoraISFT94.Domain
{
    // Forma en que entra o sale el dinero (efectivo, transferencia, etc.).
    public class MedioPago : ItemCatalogo
    {
        public MedioPago(int id, string nombre) : base(id, nombre, "medio de pago")
        {
        }
    }
}
