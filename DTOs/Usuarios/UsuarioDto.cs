using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Usuarios
{
    /// <summary>Vista segura de un usuario: nunca incluye la contraseña ni su hash.</summary>
    public record UsuarioDto(
        int Id,
        string NombreUsuario,
        string Nombre,
        string Apellido,
        bool Activo,
        IReadOnlyList<Rol> Roles)
    {
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string RolesTexto => string.Join(", ", Roles);
        public string EstadoTexto => Activo ? "Activo" : "Inactivo";

        public bool TieneRol(Rol rol) => Roles.Contains(rol);
    }
}
