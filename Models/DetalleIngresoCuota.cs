using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    /// <summary>Vincula un ingreso de cuota social con la cuota que paga (un ingreso puede pagar varias cuotas).</summary>
    public class DetalleIngresoCuota : Entidad
    {
        public DetalleIngresoCuota(Ingreso ingreso, Cuota cuota, decimal monto)
        {
            Ingreso = ingreso ?? throw new ReglaNegocioException("El detalle debe pertenecer a un ingreso.");
            Cuota = cuota ?? throw new ReglaNegocioException("El detalle debe corresponder a una cuota.");

            if (ingreso.CategoriaIngreso != CategoriaIngreso.CuotaSocial)
                throw new ReglaNegocioException("Solo un ingreso de cuota social puede pagar cuotas.");

            Monto = Validaciones.ExigirMontoPositivo(monto, "monto");

            if (Monto != cuota.Monto)
                throw new ReglaNegocioException("El monto del detalle debe cubrir exactamente el monto de la cuota.");
            if (Monto > ingreso.Monto)
                throw new ReglaNegocioException("El monto del detalle no puede superar el monto del ingreso.");
        }

        public Ingreso Ingreso { get; }

        public Cuota Cuota { get; }

        public decimal Monto { get; }
    }
}
