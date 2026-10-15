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

        /// <summary>Caja a la que se aplicó el movimiento; null mientras no se haya aplicado.</summary>
        public Caja? Caja { get; private set; }

        public abstract TipoMovimiento Tipo { get; }

        /// <summary>Categoría (ingresos) o motivo (egresos), en texto, para listados.</summary>
        public abstract string Clasificacion { get; }

        /// <summary>
        /// Aplica el movimiento a la caja y al total de su medio de pago, y lo deja ligado a esa caja.
        /// Falla sin modificar nada si la caja está cerrada, el total no corresponde o el movimiento ya se aplicó.
        /// </summary>
        public void AfectarCaja(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            ArgumentNullException.ThrowIfNull(caja);
            ArgumentNullException.ThrowIfNull(totalMedioPago);

            if (Caja is not null)
                throw new ReglaNegocioException("El movimiento ya fue registrado en una caja.");

            caja.ExigirAbierta();

            if (!ReferenceEquals(totalMedioPago.Caja, caja) || !ReferenceEquals(totalMedioPago.MedioPago, MedioPago))
                throw new ReglaNegocioException("El total por medio de pago no corresponde a la caja o al medio del movimiento.");

            Aplicar(caja, totalMedioPago);
            Caja = caja;
        }

        /// <summary>Lo que cambia según el tipo: el ingreso acredita, el egreso debita.</summary>
        protected abstract void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago);
    }
}
