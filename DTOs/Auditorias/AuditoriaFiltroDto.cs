using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Auditorias
{
    /// <summary>Filtros del registro de auditoría. Null (o texto vacío) significa "sin filtrar".</summary>
    public record AuditoriaFiltroDto(
        DateTime? Desde,
        DateTime? Hasta,
        AccionAuditoria? Accion,
        string? Texto);
}
