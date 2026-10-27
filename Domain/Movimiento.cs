using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    // Reglas comunes a ingresos y egresos. Cada subclase sabe cómo afecta a la caja: quien trabaja con un
    // Movimiento no necesita preguntar de qué tipo es.
    public abstract class Movimiento : IEntidad
    {
        public const int LargoMaximoMotivoAnulacion = 200;

        protected Movimiento(int id, Usuario usuario, DateTime fecha, decimal monto, MedioPago medioPago)
        {
            Id = id;
            Usuario = Validaciones.ExigirDato(usuario, "El movimiento debe registrarlo un usuario.");
            MedioPago = Validaciones.ExigirDato(medioPago, "El medio de pago es obligatorio.");
            Fecha = fecha;
            Monto = Validaciones.ExigirMontoPositivo(monto, "monto");
        }

        public int Id { get; private set; }

        public Usuario Usuario { get; private set; }

        public MedioPago MedioPago { get; private set; }

        public DateTime Fecha { get; private set; }

        public decimal Monto { get; private set; }

        // Caja a la que se aplicó; null mientras no se haya aplicado.
        public Caja? Caja { get; private set; }

        // Un movimiento anulado revirtió su efecto en la caja, pero queda registrado.
        public bool Anulado { get; private set; }

        public DateTime? FechaAnulacion { get; private set; }

        public Usuario? AnuladoPor { get; private set; }

        public string MotivoAnulacion { get; private set; } = string.Empty;

        public abstract TipoMovimiento Tipo { get; }

        // Categoría (ingresos) o motivo (egresos), en texto, para los listados.
        public abstract string Clasificacion { get; }

        // Aplica el movimiento a la caja y al total de su medio de pago. Falla sin modificar nada si la caja
        // está cerrada, el total no corresponde o el movimiento ya se aplicó.
        public void AfectarCaja(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            if (Caja != null)
            {
                throw new NegocioException("El movimiento ya fue registrado en una caja.");
            }
            caja.ExigirAbierta();
            ExigirTotalCorrespondiente(caja, totalMedioPago);

            Aplicar(caja, totalMedioPago);
            Caja = caja;
        }

        // Revierte el efecto en la caja y lo marca como anulado. Solo se anulan movimientos de la caja abierta:
        // una caja cerrada ya se contó y no se modifica.
        public void Anular(Usuario usuario, DateTime fecha, string motivo, CajaTotalMedioPago totalMedioPago)
        {
            Validaciones.ExigirDato(usuario, "La anulación debe registrarla un usuario.");
            string motivoValido = Validaciones.ExigirTexto(motivo, "motivo de la anulación", LargoMaximoMotivoAnulacion);
            Caja caja = ExigirAnulable();
            ExigirTotalCorrespondiente(caja, totalMedioPago);

            Revertir(caja, totalMedioPago);
            Anulado = true;
            FechaAnulacion = fecha;
            AnuladoPor = usuario;
            MotivoAnulacion = motivoValido;
        }

        // Devuelve la caja del movimiento si todavía se puede anular.
        private Caja ExigirAnulable()
        {
            if (Anulado)
            {
                throw new NegocioException("El movimiento ya está anulado.");
            }
            if (Caja == null)
            {
                throw new NegocioException("El movimiento no se registró en ninguna caja.");
            }
            if (!Caja.EstaAbierta)
            {
                throw new NegocioException("Solo se pueden anular movimientos de la caja abierta.");
            }
            return Caja;
        }

        // Lo que cambia según el tipo: el ingreso acredita y el egreso debita.
        protected abstract void Aplicar(Caja caja, CajaTotalMedioPago totalMedioPago);

        // Lo contrario de Aplicar.
        protected abstract void Revertir(Caja caja, CajaTotalMedioPago totalMedioPago);

        private void ExigirTotalCorrespondiente(Caja caja, CajaTotalMedioPago totalMedioPago)
        {
            if (!ReferenceEquals(totalMedioPago.Caja, caja) || !ReferenceEquals(totalMedioPago.MedioPago, MedioPago))
            {
                throw new NegocioException("El total por medio de pago no corresponde a la caja o al medio del movimiento.");
            }
        }
    }
}
