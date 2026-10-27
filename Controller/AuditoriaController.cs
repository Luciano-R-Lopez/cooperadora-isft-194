using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
{
    /// <summary>Consulta del registro de auditoría. Solo para administradores.</summary>
    public class AuditoriaController : BaseController
    {
        private readonly IAuditoriaService _auditoria;

        public AuditoriaController() : this(AuditoriaService.Instancia, SesionActual.Instancia) { }

        public AuditoriaController(IAuditoriaService auditoria, SesionActual sesion) : base(sesion)
        {
            _auditoria = auditoria;
        }

        public IReadOnlyList<AuditoriaDto> Obtener(AuditoriaFiltroDto filtro)
        {
            ExigirRol(Rol.Administrador);
            return _auditoria.Obtener(filtro);
        }
    }
}
