using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Domain;

namespace CooperadoraISFT94.Service
{
    /// <summary>Convierte entidades en DTOs. Las entidades no salen de la capa de servicios.</summary>
    public static class UsuarioMapper
    {
        public static UsuarioDto ADto(Usuario u) =>
            new(u.Id, u.NombreUsuario, u.Nombre, u.Apellido, u.Activo, u.Roles.ToList());
    }
}
