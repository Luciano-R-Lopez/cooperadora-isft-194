using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    // Fila de ingreso o egreso para la pantalla de movimientos.
    public class MovimientoDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public TipoMovimiento Tipo { get; set; }

        // Categoría del ingreso o motivo del egreso.
        public string Clasificacion { get; set; } = string.Empty;

        // Grupo del egreso; "-" en los ingresos.
        public string Grupo { get; set; } = string.Empty;

        // Cuotas que pagó, si es un cobro de cuotas.
        public string Detalle { get; set; } = string.Empty;

        public string MedioPago { get; set; } = string.Empty;

        public decimal Monto { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public bool Anulado { get; set; }

        public string MotivoAnulacion { get; set; } = string.Empty;

        // Los egresos se muestran en negativo.
        public decimal MontoConSigno
        {
            get { return Tipo == TipoMovimiento.Egreso ? -Monto : Monto; }
        }

        public string Estado
        {
            get { return Anulado ? "Anulado: " + MotivoAnulacion : "Vigente"; }
        }
    }
}
