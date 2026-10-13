using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Auditorias
{
    public record AuditoriaDto(
        int Id,
        DateTime Fecha,
        string Usuario,
        AccionAuditoria Accion,
        string EntidadAfectada,
        int EntidadId);
}
