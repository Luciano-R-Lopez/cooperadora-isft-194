using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IAuditoriaService
    {
        /// <summary>Deja constancia de que el usuario realizó la acción sobre la entidad indicada.</summary>
        void Registrar(int usuarioId, AccionAuditoria accion, string entidadAfectada, int entidadId);

        /// <summary>Todos los registros, del más reciente al más antiguo.</summary>
        IReadOnlyList<AuditoriaDto> ObtenerTodas();
    }
}
