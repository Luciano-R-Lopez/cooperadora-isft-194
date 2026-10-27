using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class IngresoController : BaseController
    {
        private readonly IngresoService servicio;

        public IngresoController()
        {
            servicio = new IngresoService();
        }

        public IngresoDTO Registrar(IngresoDTO dto)
        {
            ExigirRol(Rol.Operador);
            return servicio.Registrar(dto, UsuarioActualId);
        }
    }
}
