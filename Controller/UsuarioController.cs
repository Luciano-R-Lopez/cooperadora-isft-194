using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
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
            var editado = _usuarios.Editar(dto, UsuarioActualId);
            Sesion.Actualizar(editado);
            return editado;
        }

        /// <summary>Cualquier usuario logueado puede cambiar su propia contraseña.</summary>
        public void CambiarMiPassword(CambioPasswordDto dto)
        {
            ExigirAlgunRol(Enum.GetValues<Rol>());
            _usuarios.CambiarPasswordPropia(UsuarioActualId, dto);
        }

        public UsuarioDto CambiarEstado(int usuarioId, bool activo)
        {
            ExigirRol(Rol.Administrador);
            return _usuarios.CambiarEstado(usuarioId, activo, UsuarioActualId);
        }
    }
}
