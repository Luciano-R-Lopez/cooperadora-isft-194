namespace CooperadoraISFT94.Dto
{
    /// <summary>Resultado de generar las cuotas de un año para todos los socios activos.</summary>
    public record GeneracionCuotasDto(int Anio, decimal Monto, int Generadas, int YaTenian);
}
