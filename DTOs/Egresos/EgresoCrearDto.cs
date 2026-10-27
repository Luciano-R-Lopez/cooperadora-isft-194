namespace CooperadoraISFT94.DTOs.Egresos
{
    public record EgresoCrearDto(
        decimal Monto,
        int MotivoEgresoId,
        int GrupoEgresoId,
        int MedioPagoId);
}
