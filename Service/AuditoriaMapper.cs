using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    public static class AuditoriaMapper
    {
        public static AuditoriaDto ADto(Auditoria a) =>
            new(a.Id, a.Fecha, a.Usuario.NombreCompleto, a.Accion, a.EntidadAfectada, a.EntidadId, a.Detalle);
    }
}
