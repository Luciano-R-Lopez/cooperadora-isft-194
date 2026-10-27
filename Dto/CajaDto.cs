using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public record CajaDto(
        int Id,
        DateTime FechaApertura,
        DateTime? FechaCierre,
        decimal MontoInicial,
        decimal MontoTotal,
        EstadoCaja Estado)
    {
        public bool Abierta => Estado == EstadoCaja.Abierta;

        public string Descripcion =>
            $"Caja #{Id} - {FechaApertura:dd/MM/yyyy HH:mm} ({Estado.ToString().ToUpperInvariant()})";
    }
}
