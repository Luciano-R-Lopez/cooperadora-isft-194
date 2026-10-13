using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Services
{
    /// <summary>Convierte entidades en DTOs. Las entidades no salen de la capa de servicios.</summary>
    public static class SocioMapper
    {
        public static SocioDto ADto(Socio s) =>
            new(s.Id, s.Nombre, s.Apellido, s.Dni, s.Telefono, s.Email, s.Categoria,
                s.Responsable?.Id, s.Responsable?.ObtenerDescripcion());
    }
}
