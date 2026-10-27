using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    /// <summary>Valor de la cuota social de un año: el que se propone al generar cuotas de ese año.</summary>
    public class ValorCuota : Entidad
    {
        public ValorCuota(int anio, decimal monto)
        {
            if (!Validaciones.EsAnioValido(anio))
                throw new ReglaNegocioException("El año de la cuota no es válido.");
            Anio = anio;
            Monto = Validaciones.ExigirMontoPositivo(monto, "valor de la cuota");
        }

        public int Anio { get; }

        public decimal Monto { get; private set; }

        /// <summary>Cambia el valor para las cuotas que se generen de ahora en más; las ya generadas no cambian.</summary>
        public void CambiarMonto(decimal monto) =>
            Monto = Validaciones.ExigirMontoPositivo(monto, "valor de la cuota");
    }
}
