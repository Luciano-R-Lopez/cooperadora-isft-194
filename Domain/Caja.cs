using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Domain
{
    public class Caja : Entidad
    {
        public const string MensajeSinCajaAbierta =
            "No hay una caja abierta. El contador debe abrirla antes de registrar movimientos.";

        /// <summary>Una caja nace abierta y su monto total arranca en el monto inicial.</summary>
        public Caja(decimal montoInicial, DateTime fechaApertura)
        {
            MontoInicial = Validaciones.ExigirMontoNoNegativo(montoInicial, "monto inicial");
            MontoTotal = MontoInicial;
            FechaApertura = fechaApertura;
            Estado = EstadoCaja.Abierta;
        }

        public DateTime FechaApertura { get; private set; }

        public DateTime? FechaCierre { get; private set; }

        public decimal MontoInicial { get; private set; }

        /// <summary>
        /// Con la caja abierta es el monto que debería haber (inicial + ingresos - egresos).
        /// Al cerrarla pasa a ser el monto que se contó realmente.
        /// </summary>
        public decimal MontoTotal { get; private set; }

        public EstadoCaja Estado { get; private set; }

        public bool EstaAbierta => Estado == EstadoCaja.Abierta;

        /// <summary>Los movimientos solo se pueden registrar en una caja abierta.</summary>
        public void ExigirAbierta()
        {
            if (!EstaAbierta)
                throw new ReglaNegocioException(MensajeSinCajaAbierta);
        }

        /// <summary>Suma un ingreso al monto total.</summary>
        public void Acreditar(decimal monto)
        {
            ExigirAbierta();
            MontoTotal += Validaciones.ExigirMontoPositivo(monto, "monto");
        }

        /// <summary>Resta un egreso del monto total. No se puede sacar más de lo que hay en la caja.</summary>
        public void Debitar(decimal monto)
        {
            ExigirAbierta();
            Validaciones.ExigirMontoPositivo(monto, "monto");
            if (monto > MontoTotal)
                throw new ReglaNegocioException($"Saldo insuficiente: en la caja hay {MontoTotal:C2}.");
            MontoTotal -= monto;
        }

        /// <param name="montoTotal">Monto contado al cerrar.</param>
        public void CerrarCaja(DateTime fechaCierre, decimal montoTotal)
        {
            if (!EstaAbierta)
                throw new ReglaNegocioException("La caja ya está cerrada.");
            if (fechaCierre < FechaApertura)
                throw new ReglaNegocioException("La fecha de cierre no puede ser anterior a la de apertura.");

            MontoTotal = Validaciones.ExigirMontoNoNegativo(montoTotal, "monto total");
            FechaCierre = fechaCierre;
            Estado = EstadoCaja.Cerrada;
        }
    }
}
