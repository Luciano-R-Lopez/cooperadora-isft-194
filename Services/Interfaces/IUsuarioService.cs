using CooperadoraISFT94.DTOs.Usuarios;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IUsuarioService
    {
        IReadOnlyList<UsuarioDto> ObtenerTodos();
        UsuarioDto Registrar(UsuarioCrearDto dto, int actorId);

        /// <summary>Edita datos y rol del usuario.</summary>
        UsuarioDto Editar(UsuarioEditarDto dto, int actorId);

        UsuarioDto CambiarEstado(int usuarioId, bool activo, int actorId);

        /// <summary>El usuario cambia su propia contraseña; tiene que informar la actual.</summary>
        void CambiarPasswordPropia(int usuarioId, CambioPasswordDto dto);
    }
}
