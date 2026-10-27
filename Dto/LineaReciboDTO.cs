namespace CooperadoraISFT94.Dto
{
    public class LineaReciboDTO
    {
        public int CuotaId { get; set; }

        public string Socio { get; set; } = string.Empty;

        public int Anio { get; set; }

        public decimal Monto { get; set; }
    }
}
