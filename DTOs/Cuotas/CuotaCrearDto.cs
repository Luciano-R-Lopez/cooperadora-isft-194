namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>Datos para generar la cuota anual de un socio.</summary>
    public record CuotaCrearDto(
        int SocioId,
        decimal Monto,
        int Anio);
}
