using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Usuarios
{
    /// <summary>
    /// Datos editables de un usuario. El nombre de usuario no se puede cambiar.
    /// NuevaPassword vacía o null significa "no cambiar la contraseña".
    /// </summary>
    public record UsuarioEditarDto(
        int Id,
        string Nombre,
        string Apellido,
        string? NuevaPassword,
        bool Activo,
        Rol Rol);
}
