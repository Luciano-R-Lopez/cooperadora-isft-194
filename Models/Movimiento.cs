using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    /// <summary>
    /// Reglas comunes a ingresos y egresos. Cada subclase sabe cómo afecta a la caja (polimorfismo):
    /// quien trabaja con un Movimiento no necesita preguntar de qué tipo es.
    /// </summary>
    public abstract class Movimiento : Entidad
    {
        protected Movimiento(Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago)
        {
            Usuario = usuario ?? throw new ReglaNegocioException("El movimiento debe registrarlo un usuario.");
            MedioPago = medioPago ?? throw new ReglaNegocioException("El medio de pago es obligatorio.");
            Fecha = fecha;
            Monto = Validaciones.ExigirMontoPositivo(monto, "monto");
        }

        public Usuario Usuario { get; }

        public MedioPago MedioPago { get; }

        public DateTime Fecha { get; }

        public decimal Monto { get; }

        public abstract TipoMovimiento Tipo { get; }

        /// <summary>Categoría (ingresos) o motivo (egresos), en texto, para listados.</summary>
        public abstract string Clasificacion { get; }

        /// <summary>
        /// Aplica el movimiento a la caja y al total de su medio de pago.
        /// Falla sin modificar nada si la caja está cerrada o el total no corresponde.
        /// </summary>
        public abstract void AfectarCaja(Caja caja, CajaTotalMedioPago totalMedioPago);

        /// <summary>Controles previos a afectar la caja, comunes a todos los tipos de movimiento.</summary>
        protected void ValidarDestino(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            ArgumentNullException.ThrowIfNull(caja);
            ArgumentNullException.ThrowIfNull(totalMedioPago);

            caja.ExigirAbierta();

            if (!ReferenceEquals(totalMedioPago.Caja, caja) || !ReferenceEquals(totalMedioPago.MedioPago, MedioPago))
                throw new ReglaNegocioException("El total por medio de pago no corresponde a la caja o al medio del movimiento.");
        }
    }
}
