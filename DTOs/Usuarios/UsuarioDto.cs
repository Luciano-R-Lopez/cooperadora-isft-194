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
        Rol Rol)
    {
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public string RolTexto => Rol.ToString();
        public string EstadoTexto => Activo ? "Activo" : "Inactivo";
    }
}
