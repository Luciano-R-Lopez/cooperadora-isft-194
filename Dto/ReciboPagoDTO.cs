namespace CooperadoraISFT94.Dto
{
    // Una o varias cuotas cobradas juntas en un solo ingreso. El número es el del ingreso.
    public class ReciboPagoDTO
    {
        public int Numero { get; set; }

        public DateTime Fecha { get; set; }

        public List<LineaReciboDTO> Cuotas { get; set; } = new List<LineaReciboDTO>();

        public string MedioPago { get; set; } = string.Empty;

        // Responsable que pagó por sus socios a cargo; vacío si pagó el propio socio.
        public string PagadoPor { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public decimal Monto
        {
            get { return Cuotas.Sum(c => c.Monto); }
        }

        public string Socio
        {
            get { return string.Join("; ", Cuotas.Select(c => c.Socio).Distinct()); }
        }
    }
}
