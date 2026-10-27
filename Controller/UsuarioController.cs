using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class UsuarioController : BaseController
    {
        private readonly UsuarioService servicio;

        public UsuarioController()
        {
            servicio = new UsuarioService();
        }

        public List<UsuarioDTO> ObtenerTodos()
        {
            ExigirRol(Rol.Administrador);
            return servicio.ObtenerTodos();
        }

        public UsuarioDTO ObtenerPorId(int id)
        {
            ExigirRol(Rol.Administrador);
            return servicio.ObtenerPorId(id);
        }

        // Crea o edita según dto.Id. Si se editó al usuario que inició sesión, se actualiza la sesión.
        public UsuarioDTO Guardar(UsuarioDTO dto)
        {
            ExigirRol(Rol.Administrador);
            UsuarioDTO guardado = servicio.Guardar(dto, UsuarioActualId);
            Sesion.Actualizar(guardado);
            return guardado;
        }

        public UsuarioDTO CambiarEstado(int usuarioId, bool activo)
        {
            ExigirRol(Rol.Administrador);
            return servicio.CambiarEstado(usuarioId, activo, UsuarioActualId);
        }

        // Cualquier usuario puede cambiar su propia contraseña.
        public void CambiarMiPassword(CambioPasswordDTO dto)
        {
            ExigirRol(Enum.GetValues<Rol>());
            servicio.CambiarPasswordPropia(UsuarioActualId, dto);
        }
    }
}
