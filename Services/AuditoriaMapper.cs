using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    public static class AuditoriaMapper
    {
        public static AuditoriaDto ADto(Auditoria a) =>
            new(a.Id, a.Fecha, a.Usuario.NombreCompleto, a.Accion, a.EntidadAfectada, a.EntidadId);
    }
}
