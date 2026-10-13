using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Ingresos
{
    public record IngresoCrearDto(
        decimal Monto,
        CategoriaIngreso CategoriaIngreso,
        int MedioPagoId);
}
