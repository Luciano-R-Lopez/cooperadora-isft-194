using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public record IngresoDto(
        int Id,
        DateTime Fecha,
        decimal Monto,
        CategoriaIngreso CategoriaIngreso,
        string MedioPago);
}
