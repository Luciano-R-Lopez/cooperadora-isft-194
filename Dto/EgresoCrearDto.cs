namespace CooperadoraISFT94.Dto
{
    public record EgresoCrearDto(
        decimal Monto,
        int MotivoEgresoId,
        int GrupoEgresoId,
        int MedioPagoId);
}
