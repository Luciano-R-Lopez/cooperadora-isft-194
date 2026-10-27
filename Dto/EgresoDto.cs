namespace CooperadoraISFT94.Dto
{
    public record EgresoDto(
        int Id,
        DateTime Fecha,
        decimal Monto,
        string Motivo,
        string Grupo,
        string MedioPago);
}
