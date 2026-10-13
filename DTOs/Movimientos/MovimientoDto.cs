using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Movimientos
{
    /// <summary>Fila unificada de ingreso o egreso para el dashboard de movimientos.</summary>
    public record MovimientoDto(
        TipoMovimiento Tipo,
        DateTime Fecha,
        string Clasificacion,
        string Detalle,
        string MedioPago,
        decimal Monto,
        string Usuario)
    {
        /// <summary>Los egresos se muestran en negativo.</summary>
        public decimal MontoConSigno => Tipo == TipoMovimiento.Egreso ? -Monto : Monto;
    }
}
