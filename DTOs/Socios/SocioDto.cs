using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Socios
{
    /// <summary>Datos de un socio para mostrar en las vistas.</summary>
    public record SocioDto(
        int Id,
        string Nombre,
        string Apellido,
        string Dni,
        string Telefono,
        string Email,
        Categoria Categoria)
    {
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
