namespace CooperadoraISFT94.DTOs.Cajas
{
    /// <summary>
    /// Totales calculados de una caja (sin los movimientos anulados). MontoDeclarado y Diferencia solo
    /// tienen valor cuando se informó el monto contado (en una caja cerrada, el que se contó al cerrar).
    /// </summary>
    public record ResumenCajaDto(
        CajaDto Caja,
        decimal MontoInicial,
        decimal TotalIngresos,
        decimal TotalEgresos,
        decimal MontoEsperado,
        decimal? MontoDeclarado,
        decimal? Diferencia,
        IReadOnlyList<TotalMedioPagoDto> TotalesPorMedio,
        int CantidadMovimientos,
        int CantidadAnulados);
}
