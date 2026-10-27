using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Domain
{
    // Valor de la cuota social de un año: el que se propone al generar las cuotas de ese año.
    public class ValorCuota : IEntidad
    {
        private decimal monto;

        public ValorCuota(int id, int anio, decimal monto)
        {
            Id = id;
            Anio = Validaciones.ExigirAnio(anio);
            Monto = monto;
        }

        public int Id { get; private set; }

        public int Anio { get; private set; }

        // Cambiarlo afecta a las cuotas que se generen de ahora en más; las ya generadas no cambian.
        public decimal Monto
        {
            get { return monto; }
            set { monto = Validaciones.ExigirMontoPositivo(value, "valor de la cuota"); }
        }
    }
}
