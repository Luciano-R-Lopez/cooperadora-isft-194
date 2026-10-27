using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
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
        string? Responsable,
        bool Activo)
    {
        public string NombreCompleto => $"{Nombre} {Apellido}";

        /// <summary>"Apellido, Nombre (DNI)" para desplegables.</summary>
        public string Descripcion => $"{Apellido}, {Nombre} ({Dni})";

        public string ResponsableTexto => Responsable ?? "-";

        public string EstadoTexto => Activo ? "Activo" : "De baja";
    }
}
