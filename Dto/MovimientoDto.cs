using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    /// <summary>Fila unificada de ingreso o egreso para el dashboard de movimientos.</summary>
    public record MovimientoDto(
        int Id,
        TipoMovimiento Tipo,
        DateTime Fecha,
        string Clasificacion,
        string Grupo,
        string Detalle,
        string MedioPago,
        decimal Monto,
        string Usuario,
        bool Anulado,
        string MotivoAnulacion)
    {
        /// <summary>Los egresos se muestran en negativo.</summary>
        public decimal MontoConSigno => Tipo == TipoMovimiento.Egreso ? -Monto : Monto;

        public string EstadoTexto => Anulado ? $"Anulado: {MotivoAnulacion}" : "Vigente";
    }
}
