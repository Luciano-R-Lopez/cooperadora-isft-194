using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Caja : IEntidad
    {
        public const string MensajeSinCajaAbierta =
            "No hay una caja abierta. El contador debe abrirla antes de registrar movimientos.";

        // Una caja nace abierta y su monto total arranca en el monto inicial.
        public Caja(int id, decimal montoInicial, DateTime fechaApertura)
        {
            Id = id;
            MontoInicial = Validaciones.ExigirMontoNoNegativo(montoInicial, "monto inicial");
            MontoTotal = MontoInicial;
            FechaApertura = fechaApertura;
            Estado = EstadoCaja.Abierta;
        }

        public int Id { get; private set; }

        public DateTime FechaApertura { get; private set; }

        public DateTime? FechaCierre { get; private set; }

        public decimal MontoInicial { get; private set; }

        // Con la caja abierta es lo que debería haber (inicial + ingresos - egresos).
        // Al cerrarla pasa a ser lo que se contó realmente.
        public decimal MontoTotal { get; private set; }

        public EstadoCaja Estado { get; private set; }

        public bool EstaAbierta
        {
            get { return Estado == EstadoCaja.Abierta; }
        }

        public void ExigirAbierta()
        {
            if (!EstaAbierta)
            {
                throw new NegocioException(MensajeSinCajaAbierta);
            }
        }

        public void Acreditar(decimal monto)
        {
            ExigirAbierta();
            MontoTotal += Validaciones.ExigirMontoPositivo(monto, "monto");
        }

        // No se puede sacar más de lo que hay en la caja.
        public void Debitar(decimal monto)
        {
            ExigirAbierta();
            Validaciones.ExigirMontoPositivo(monto, "monto");
            if (monto > MontoTotal)
            {
                throw new NegocioException("Saldo insuficiente: en la caja hay " + Formato.Monto(MontoTotal) + ".");
            }
            MontoTotal -= monto;
        }

        // montoContado: el dinero que se contó al cerrar.
        public void Cerrar(DateTime fechaCierre, decimal montoContado)
        {
            if (!EstaAbierta)
            {
                throw new NegocioException("La caja ya está cerrada.");
            }
            if (fechaCierre < FechaApertura)
            {
                throw new NegocioException("La fecha de cierre no puede ser anterior a la de apertura.");
            }
            MontoTotal = Validaciones.ExigirMontoNoNegativo(montoContado, "monto contado");
            FechaCierre = fechaCierre;
            Estado = EstadoCaja.Cerrada;
        }
    }
}
