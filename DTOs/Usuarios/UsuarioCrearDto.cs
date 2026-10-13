using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Usuarios
{
    /// <summary>Datos que ingresa el administrador para dar de alta un usuario.</summary>
    public record UsuarioCrearDto(
        string NombreUsuario,
        string Nombre,
        string Apellido,
        string Password,
        bool Activo,
        Rol Rol);
}
