using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public class IngresoDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public CategoriaIngreso CategoriaIngreso { get; set; }

        public int MedioPagoId { get; set; }

        public string MedioPago { get; set; } = string.Empty;
    }
}
