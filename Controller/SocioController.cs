using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class SocioController : BaseController
    {
        private readonly SocioService servicio;

        public SocioController()
        {
            servicio = new SocioService();
        }

        // Crea o edita según dto.Id; devuelve el id del socio guardado.
        public int Guardar(SocioDTO dto)
        {
            ExigirRol(Rol.Operador);
            return servicio.Guardar(dto, UsuarioActualId);
        }

        public SocioDTO CambiarEstado(int socioId, bool activo)
        {
            ExigirRol(Rol.Operador);
            return servicio.CambiarEstado(socioId, activo, UsuarioActualId);
        }

        public List<SocioDTO> ObtenerTodos()
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerTodos();
        }

        public SocioDTO ObtenerPorId(int id)
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerPorId(id);
        }

        public List<SocioDTO> ObtenerResponsables()
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerResponsables();
        }

        public List<SocioDTO> ObtenerACargo(int responsableId)
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerACargo(responsableId);
        }
    }
}
