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
            Id = id;
            Ingreso = ExigirIngresoDeCuota(ingreso);
            Cuota = Validaciones.ExigirDato(cuota, "El detalle debe tener una cuota.");
            Monto = ExigirMontoDeLaCuota(monto);
            PagadaPor = ExigirPagador(pagadaPor);
        }

        // Para leer un pago ya guardado: quien lo pagó pudo dejar de ser el responsable del socio después.
        internal static DetalleIngresoCuota Restaurar(int id, Ingreso ingreso, Cuota cuota, decimal monto, Socio? pagadaPor)
        {
            DetalleIngresoCuota detalle = new DetalleIngresoCuota(id, ingreso, cuota, monto, null);
            detalle.PagadaPor = pagadaPor;
            return detalle;
        }

        public int Id { get; private set; }

        public Ingreso Ingreso { get; private set; }

        public Cuota Cuota { get; private set; }

        public decimal Monto { get; private set; }

        public Socio? PagadaPor { get; private set; }

        private static Ingreso ExigirIngresoDeCuota(Ingreso ingreso)
        {
            Validaciones.ExigirDato(ingreso, "El detalle debe tener un ingreso.");
            if (ingreso.CategoriaIngreso != CategoriaIngreso.CuotaSocial)
            {
                throw new NegocioException("Solo un ingreso de cuota social puede pagar cuotas.");
            }
            return ingreso;
        }

        // Una cuota se paga entera, con un ingreso que alcance.
        private decimal ExigirMontoDeLaCuota(decimal monto)
        {
            Validaciones.ExigirMontoPositivo(monto, "monto");
            if (monto != Cuota.Monto)
            {
                throw new NegocioException("El monto del detalle debe cubrir exactamente el monto de la cuota.");
            }
            if (monto > Ingreso.Monto)
            {
                throw new NegocioException("El monto del detalle no puede superar el monto del ingreso.");
            }
            return monto;
        }

        private Socio? ExigirPagador(Socio? pagadaPor)
        {
            if (pagadaPor != null && !ReferenceEquals(pagadaPor, Cuota.Socio.Responsable))
            {
                throw new NegocioException("Solo el responsable del socio puede pagar su cuota en su nombre.");
            }
            return pagadaPor;
        }
    }
}
