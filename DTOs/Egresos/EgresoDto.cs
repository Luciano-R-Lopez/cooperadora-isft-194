namespace CooperadoraISFT94.DTOs.Egresos
{
    public record EgresoDto(
        int Id,
        DateTime Fecha,
        decimal Monto,
        string Motivo,
        string MedioPago);
}
