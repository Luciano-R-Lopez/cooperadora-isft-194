using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public record IngresoCrearDto(
        decimal Monto,
        CategoriaIngreso CategoriaIngreso,
        int MedioPagoId);
}
