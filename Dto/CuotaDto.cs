using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
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

        public string DescripcionConMonto => $"Cuota {Anio} - {Monto:C2}";
    }
}
