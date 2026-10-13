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
    }
}
