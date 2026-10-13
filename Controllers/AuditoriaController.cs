using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
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

        public IReadOnlyList<AuditoriaDto> ObtenerTodas()
        {
            ExigirRol(Rol.Administrador);
            return _auditoria.ObtenerTodas();
        }
    }
}
