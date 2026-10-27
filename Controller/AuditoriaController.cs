using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class AuditoriaController : BaseController
    {
        private readonly AuditoriaService servicio;

        public AuditoriaController()
        {
            servicio = new AuditoriaService();
        }

        public List<AuditoriaDTO> Obtener(AuditoriaFiltroDTO filtro)
        {
            ExigirRol(Rol.Administrador);
            return servicio.Obtener(filtro);
        }
    }
}
