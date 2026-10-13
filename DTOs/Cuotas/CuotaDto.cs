using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>Datos de una cuota para mostrar en las vistas.</summary>
    public record CuotaDto(
        int Id,
        int SocioId,
        int Anio,
        decimal Monto,
        EstadoCuota Estado)
    {
        public string Descripcion => $"Cuota {Anio}";
    }
}
