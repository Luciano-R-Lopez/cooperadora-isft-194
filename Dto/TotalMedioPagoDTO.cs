namespace CooperadoraISFT94.Dto
{
    // Saldo neto (ingresos - egresos) de la caja para un medio de pago.
    public class TotalMedioPagoDTO
    {
        public string MedioPago { get; set; } = string.Empty;

        public decimal Monto { get; set; }
    }
}
