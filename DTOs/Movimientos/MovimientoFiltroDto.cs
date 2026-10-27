using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Movimientos
{
    /// <summary>
    /// Filtros del dashboard. Tipo, MedioPagoId y GrupoEgresoId en null significan "todos".
    /// Elegir un grupo deja solo los egresos de ese grupo.
    /// </summary>
    public record MovimientoFiltroDto(
        int CajaId,
        TipoMovimiento? Tipo,
        int? MedioPagoId,
        string? Texto,
        int? GrupoEgresoId = null);
}
