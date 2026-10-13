using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Models
{
    /// <summary>
    /// Saldo neto (ingresos - egresos) de una caja para un medio de pago.
    /// Puede ser negativo: por ejemplo, un egreso por transferencia sin ingresos por ese medio.
    /// </summary>
    public class CajaTotalMedioPago : Entidad
    {
        public CajaTotalMedioPago(Caja caja, MedioPago medioPago)
        {
            Caja = caja ?? throw new ReglaNegocioException("El total debe pertenecer a una caja.");
            MedioPago = medioPago ?? throw new ReglaNegocioException("El total debe tener un medio de pago.");
        }

        public Caja Caja { get; }

        public MedioPago MedioPago { get; }

        public decimal Monto { get; private set; }

        public void Acreditar(decimal monto) =>
            Monto += Validaciones.ExigirMontoPositivo(monto, "monto");

        public void Debitar(decimal monto) =>
            Monto -= Validaciones.ExigirMontoPositivo(monto, "monto");
    }
}
