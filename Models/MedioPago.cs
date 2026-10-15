namespace CooperadoraISFT94.Models
{
    /// <summary>Forma en que se cobra o se paga (efectivo, transferencia...).</summary>
    public class MedioPago : ItemCatalogo
    {
        public MedioPago(string nombre) : base(nombre, "medio de pago") { }
    }
}
