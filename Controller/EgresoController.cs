using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class EgresoController : BaseController
    {
        private readonly EgresoService servicio;

        public EgresoController()
        {
            servicio = new EgresoService();
        }

        public EgresoDTO Registrar(EgresoDTO dto)
        {
            ExigirRol(Rol.Contador);
            return servicio.Registrar(dto, UsuarioActualId);
        }
    }
}
