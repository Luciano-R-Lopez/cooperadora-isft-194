using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Service.Interfaces
{
    public interface IAuditoriaService
    {
        /// <summary>Deja constancia de que el usuario realizó la acción sobre la entidad indicada.</summary>
        /// <param name="detalle">Qué cambió, en texto (opcional).</param>
        void Registrar(int usuarioId, AccionAuditoria accion, string entidadAfectada, int entidadId, string? detalle = null);

        /// <summary>Registros que cumplen los filtros, del más reciente al más antiguo.</summary>
        IReadOnlyList<AuditoriaDto> Obtener(AuditoriaFiltroDto filtro);
    }
}
