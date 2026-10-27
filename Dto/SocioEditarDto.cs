using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    /// <summary>Datos nuevos de un socio existente. ResponsableId null = sin responsable.</summary>
    public record SocioEditarDto(
        int Id,
        string Nombre,
        string Apellido,
        string Dni,
        string Telefono,
        string Email,
        Categoria Categoria,
        int? ResponsableId);
}
