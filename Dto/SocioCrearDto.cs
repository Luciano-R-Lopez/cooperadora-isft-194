using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    /// <summary>Datos que ingresa el operador para inscribir un socio nuevo. ResponsableId null = sin responsable.</summary>
    public record SocioCrearDto(
        string Nombre,
        string Apellido,
        string Dni,
        string Telefono,
        string Email,
        Categoria Categoria,
        int? ResponsableId);
}
