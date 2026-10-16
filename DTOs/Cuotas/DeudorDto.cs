namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>
    /// Fila de la lista de morosos: un socio con sus cuotas pendientes (años y total) y quién las paga
    /// (él mismo o su responsable).
    /// </summary>
    public record DeudorDto(
        int SocioId,
        string Apellido,
        string Nombre,
        string Dni,
        string Telefono,
        string Email,
        int? ResponsableId,
        string? Responsable,
        bool SocioActivo,
        IReadOnlyList<int> Anios,
        decimal Total)
    {
        public string NombreCompleto => $"{Apellido}, {Nombre}";

        public string AniosTexto => string.Join(", ", Anios);

        public int CantidadCuotas => Anios.Count;

        /// <summary>A quién hay que cobrarle: el responsable si tiene, si no el propio socio.</summary>
        public string Paga => Responsable is null ? "El socio" : $"Su responsable: {Responsable}";

        public string EstadoTexto => SocioActivo ? "Activo" : "De baja";
    }
}
