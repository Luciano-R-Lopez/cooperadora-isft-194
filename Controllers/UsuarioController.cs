using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    /// <summary>Gestión de usuarios y roles. Solo para administradores.</summary>
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarios;

        public UsuarioController() : this(UsuarioService.Instancia, SesionActual.Instancia) { }

        public UsuarioController(IUsuarioService usuarios, SesionActual sesion) : base(sesion)
        {
            _usuarios = usuarios;
        }

        public IReadOnlyList<UsuarioDto> ObtenerTodos()
        {
            ExigirRol(Rol.Administrador);
            return _usuarios.ObtenerTodos();
        }

        public UsuarioDto Registrar(UsuarioCrearDto dto)
        {
            ExigirRol(Rol.Administrador);
            return _usuarios.Registrar(dto, UsuarioActualId);
        }

        public UsuarioDto Editar(UsuarioEditarDto dto)
        {
            ExigirRol(Rol.Administrador);
            return _usuarios.Editar(dto, UsuarioActualId);
        }

        public UsuarioDto CambiarEstado(int usuarioId, bool activo)
        {
            ExigirRol(Rol.Administrador);
            return _usuarios.CambiarEstado(usuarioId, activo, UsuarioActualId);
        }
    }
}
