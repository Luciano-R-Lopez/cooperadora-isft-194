namespace CooperadoraISFT94.Dto
{
    /// <summary>Saldo neto (ingresos - egresos) de la caja para un medio de pago.</summary>
    public record TotalMedioPagoDto(string MedioPago, decimal Monto);
}
