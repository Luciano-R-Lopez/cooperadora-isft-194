namespace CooperadoraISFT94.Dto
{
    /// <summary>Fila del listado de morosos: una cuota pendiente con los datos de contacto del socio.</summary>
    public record CuotaPendienteDto(
        int CuotaId,
        int SocioId,
        string Apellido,
        string Nombre,
        string Dni,
        string Telefono,
        string Email,
        string Responsable,
        bool SocioActivo,
        int Anio,
        decimal Monto);
}
