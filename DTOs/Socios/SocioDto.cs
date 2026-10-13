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
        Categoria Categoria,
        int? ResponsableId,
        string? Responsable)
    {
        public string NombreCompleto => $"{Nombre} {Apellido}";

        /// <summary>"Apellido, Nombre (DNI)" para desplegables.</summary>
        public string Descripcion => $"{Apellido}, {Nombre} ({Dni})";

        public string ResponsableTexto => Responsable ?? "-";
    }
}
