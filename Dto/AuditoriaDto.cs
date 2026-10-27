using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public record AuditoriaDto(
        int Id,
        DateTime Fecha,
        string Usuario,
        AccionAuditoria Accion,
        string EntidadAfectada,
        int EntidadId,
        string Detalle);
}
