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

        public const int LargoMaximoMotivoAnulacion = 200;

        /// <summary>True si se anuló: su efecto en la caja se revirtió, pero queda registrado.</summary>
        public bool Anulado { get; private set; }

        public DateTime? FechaAnulacion { get; private set; }

        public Usuario? AnuladoPor { get; private set; }

        public string MotivoAnulacion { get; private set; } = string.Empty;

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

        /// <summary>
        /// Revierte el efecto del movimiento en su caja y lo marca como anulado. Solo se pueden anular
        /// movimientos de la caja abierta: una caja cerrada ya se contó y no se modifica.
        /// </summary>
        public void Anular(Usuario usuario, DateTime fecha, string motivo, CajaTotalMedioPago totalMedioPago)
        {
            ArgumentNullException.ThrowIfNull(totalMedioPago);
            var usuarioValido = usuario ?? throw new ReglaNegocioException("La anulación debe registrarla un usuario.");
            var motivoValido = Validaciones.ExigirTexto(motivo, "motivo de la anulación", LargoMaximoMotivoAnulacion);

            if (Anulado)
                throw new ReglaNegocioException("El movimiento ya está anulado.");
            if (Caja is null)
                throw new ReglaNegocioException("El movimiento no se registró en ninguna caja.");
            if (!Caja.EstaAbierta)
                throw new ReglaNegocioException("Solo se pueden anular movimientos de la caja abierta.");
            if (!ReferenceEquals(totalMedioPago.Caja, Caja) || !ReferenceEquals(totalMedioPago.MedioPago, MedioPago))
                throw new ReglaNegocioException("El total por medio de pago no corresponde a la caja o al medio del movimiento.");

            Revertir(Caja, totalMedioPago);

            Anulado = true;
            FechaAnulacion = fecha;
            AnuladoPor = usuarioValido;
            MotivoAnulacion = motivoValido;
        }

        /// <summary>Lo que cambia según el tipo: el ingreso acredita, el egreso debita.</summary>
        protected abstract void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago);

        /// <summary>Lo contrario de <see cref="Aplicar"/>: el ingreso anulado debita, el egreso anulado acredita.</summary>
        protected abstract void Revertir(Caja caja, CajaTotalMedioPago totalMedioPago);
    }
}
