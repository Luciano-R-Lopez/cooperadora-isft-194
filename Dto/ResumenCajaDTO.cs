namespace CooperadoraISFT94.Dto
{
    // Totales de una caja sin los movimientos anulados. MontoDeclarado y Diferencia solo tienen valor
    // cuando se informó el monto contado (en una caja cerrada, el que se contó al cerrarla).
    public class ResumenCajaDTO
    {
        public CajaDTO Caja { get; set; } = new CajaDTO();

        public decimal MontoInicial { get; set; }

        public decimal TotalIngresos { get; set; }

        public decimal TotalEgresos { get; set; }

        public decimal MontoEsperado { get; set; }

        public decimal? MontoDeclarado { get; set; }

        public decimal? Diferencia { get; set; }

        public List<TotalMedioPagoDTO> TotalesPorMedio { get; set; } = new List<TotalMedioPagoDTO>();

        public int CantidadMovimientos { get; set; }

        public int CantidadAnulados { get; set; }
    }
}
