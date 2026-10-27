namespace CooperadoraISFT94.Dto
{
    // Resultado de generar las cuotas de un año para todos los socios activos.
    public class GeneracionCuotasDTO
    {
        public int Anio { get; set; }

        public decimal Monto { get; set; }

        public int Generadas { get; set; }

        public int YaTenian { get; set; }
    }
}
