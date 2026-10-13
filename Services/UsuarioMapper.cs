using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    /// <summary>Convierte entidades en DTOs. Las entidades no salen de la capa de servicios.</summary>
    public static class UsuarioMapper
    {
        public static UsuarioDto ADto(Usuario u) =>
            new(u.Id, u.NombreUsuario, u.Nombre, u.Apellido, u.Activo, u.Rol);
    }
}
