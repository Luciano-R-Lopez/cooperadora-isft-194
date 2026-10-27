using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    // Saldo neto (ingresos - egresos) de una caja para un medio de pago.
    // Puede ser negativo: por ejemplo, un egreso por transferencia sin ingresos por ese medio.
    public class CajaTotalMedioPago : IEntidad
    {
        public CajaTotalMedioPago(int id, Caja caja, MedioPago medioPago)
        {
            if (caja == null || medioPago == null)
            {
                throw new NegocioException("El total debe pertenecer a una caja y a un medio de pago.");
            }
            Id = id;
            Caja = caja;
            MedioPago = medioPago;
        }

        public int Id { get; private set; }

        public Caja Caja { get; private set; }

        public MedioPago MedioPago { get; private set; }

        public decimal Monto { get; private set; }

        public void Acreditar(decimal monto)
        {
            Monto += Validaciones.ExigirMontoPositivo(monto, "monto");
        }

        public void Debitar(decimal monto)
        {
            Monto -= Validaciones.ExigirMontoPositivo(monto, "monto");
        }
    }
}
