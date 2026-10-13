using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Ingresos
{
    public record IngresoDto(
        int Id,
        DateTime Fecha,
        decimal Monto,
        CategoriaIngreso CategoriaIngreso,
        string MedioPago);
}
