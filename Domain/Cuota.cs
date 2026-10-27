using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Cuota : IEntidad
    {
        // Una cuota nace pendiente; pasa a pagada al cobrarla y vuelve a pendiente solo si se anula ese cobro.
        public Cuota(int id, Socio socio, decimal monto, int anio)
        {
            if (socio == null)
            {
                throw new NegocioException("La cuota debe pertenecer a un socio.");
            }
            Id = id;
            Socio = socio;
            Monto = Validaciones.ExigirMontoPositivo(monto, "monto de la cuota");
            Anio = Validaciones.ExigirAnio(anio);
            Estado = EstadoCuota.Pendiente;
        }

        public int Id { get; private set; }

        public Socio Socio { get; private set; }

        public decimal Monto { get; private set; }

        public int Anio { get; private set; }

        public EstadoCuota Estado { get; private set; }

        public bool EstaPagada
        {
            get { return Estado == EstadoCuota.Pagada; }
        }

        public void ValidarPagable()
        {
            if (EstaPagada)
            {
                throw new NegocioException("La cuota " + Anio + " de " + Socio.ObtenerDescripcion() + " ya está pagada.");
            }
        }

        public void MarcarPagada()
        {
            ValidarPagable();
            Estado = EstadoCuota.Pagada;
        }

        // Vuelve a pendiente porque se anuló el ingreso con que se pagó.
        public void RevertirPago()
        {
            if (!EstaPagada)
            {
                throw new NegocioException("La cuota no está pagada.");
            }
            Estado = EstadoCuota.Pendiente;
        }
    }
}
