using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Movimientos
{
    /// <summary>Filtros del dashboard. Tipo y MedioPagoId en null significan "todos".</summary>
    public record MovimientoFiltroDto(
        int CajaId,
        TipoMovimiento? Tipo,
        int? MedioPagoId,
        string? Texto);
}
