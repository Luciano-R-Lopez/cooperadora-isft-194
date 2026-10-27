namespace CooperadoraISFT94.Dto
{
    public class EgresoDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Monto { get; set; }

        public int MotivoEgresoId { get; set; }

        public string Motivo { get; set; } = string.Empty;

        public int GrupoEgresoId { get; set; }

        public string Grupo { get; set; } = string.Empty;

        public int MedioPagoId { get; set; }

        public string MedioPago { get; set; } = string.Empty;
    }
}
