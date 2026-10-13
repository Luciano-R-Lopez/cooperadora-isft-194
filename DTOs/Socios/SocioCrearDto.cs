using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Socios
{
    /// <summary>Datos que ingresa el operador para inscribir un socio nuevo.</summary>
    public record SocioCrearDto(
        string Nombre,
        string Apellido,
        string Dni,
        string Telefono,
        string Email,
        Categoria Categoria);
}
