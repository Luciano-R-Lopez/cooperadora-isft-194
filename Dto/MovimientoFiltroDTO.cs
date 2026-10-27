using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    // Tipo, MedioPagoId y GrupoEgresoId en null significan "todos". Elegir un grupo deja solo los egresos.
    public class MovimientoFiltroDTO
    {
        public int CajaId { get; set; }

        public TipoMovimiento? Tipo { get; set; }

        public int? MedioPagoId { get; set; }

        public int? GrupoEgresoId { get; set; }

        public string Texto { get; set; } = string.Empty;
    }
}
