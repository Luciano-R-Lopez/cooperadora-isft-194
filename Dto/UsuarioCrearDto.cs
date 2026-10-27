using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    /// <summary>Datos que ingresa el administrador para dar de alta un usuario.</summary>
    public record UsuarioCrearDto(
        string NombreUsuario,
        string Nombre,
        string Apellido,
        string Password,
        bool Activo,
        IReadOnlyCollection<Rol> Roles);
}
