using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    // Une un ingreso de cuota social con la cuota que paga (un ingreso puede pagar varias cuotas).
    public class DetalleIngresoCuota : IEntidad
    {
        // pagadaPor: el responsable del socio, si fue él quien pagó; null si pagó el propio socio.
        public DetalleIngresoCuota(int id, Ingreso ingreso, Cuota cuota, decimal monto, Socio? pagadaPor)
        {
            if (ingreso == null || cuota == null)
            {
                throw new NegocioException("El detalle debe tener un ingreso y una cuota.");
            }
            if (ingreso.CategoriaIngreso != CategoriaIngreso.CuotaSocial)
            {
                throw new NegocioException("Solo un ingreso de cuota social puede pagar cuotas.");
            }
            Validaciones.ExigirMontoPositivo(monto, "monto");
            if (monto != cuota.Monto)
            {
                throw new NegocioException("El monto del detalle debe cubrir exactamente el monto de la cuota.");
            }
            if (monto > ingreso.Monto)
            {
                throw new NegocioException("El monto del detalle no puede superar el monto del ingreso.");
            }
            if (pagadaPor != null && !ReferenceEquals(pagadaPor, cuota.Socio.Responsable))
            {
                throw new NegocioException("Solo el responsable del socio puede pagar su cuota en su nombre.");
            }

            Id = id;
            Ingreso = ingreso;
            Cuota = cuota;
            Monto = monto;
            PagadaPor = pagadaPor;
        }

        public int Id { get; private set; }

        public Ingreso Ingreso { get; private set; }

        public Cuota Cuota { get; private set; }

        public decimal Monto { get; private set; }

        public Socio? PagadaPor { get; private set; }
    }
}
