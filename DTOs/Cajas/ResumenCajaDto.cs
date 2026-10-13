namespace CooperadoraISFT94.DTOs.Cajas
{
    /// <summary>
    /// Totales calculados de una caja. MontoDeclarado y Diferencia solo tienen valor
    /// cuando se informó el monto contado.
    /// </summary>
    public record ResumenCajaDto(
        int CajaId,
        decimal MontoInicial,
        decimal TotalIngresos,
        decimal TotalEgresos,
        decimal MontoEsperado,
        decimal? MontoDeclarado,
        decimal? Diferencia,
        IReadOnlyList<TotalMedioPagoDto> TotalesPorMedio);
}
