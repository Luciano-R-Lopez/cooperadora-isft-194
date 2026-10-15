namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>Datos del recibo de un pago de cuota. El número es el del ingreso registrado en la caja.</summary>
    public record ReciboPagoDto(
        int Numero,
        DateTime Fecha,
        int CuotaId,
        string Socio,
        int Anio,
        decimal Monto,
        string MedioPago,
        string? PagadoPor,
        string Usuario);
}
