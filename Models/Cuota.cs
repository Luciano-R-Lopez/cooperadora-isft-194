using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Cuota : Entidad
    {
        /// <summary>Una cuota nace siempre pendiente; después solo cambia de estado con CambiarEstado.</summary>
        public Cuota(Socio socio, decimal monto, int anio)
        {
            Socio = socio ?? throw new ReglaNegocioException("La cuota debe pertenecer a un socio.");
            Monto = Validaciones.ExigirMontoPositivo(monto, "monto");

            if (!Validaciones.EsAnioValido(anio))
                throw new ReglaNegocioException("El año de la cuota no es válido.");
            Anio = anio;

            Estado = EstadoCuota.Pendiente;
        }

        public Socio Socio { get; }

        public decimal Monto { get; }

        public int Anio { get; }

        public EstadoCuota Estado { get; private set; }

        public bool EstaPagada => Estado == EstadoCuota.Pagada;

        /// <summary>Lanza el error correspondiente si la cuota no se puede pagar (por ejemplo, ya está pagada).</summary>
        public void ValidarPagable()
        {
            if (EstaPagada)
                throw new ReglaNegocioException("La cuota ya está pagada.");
        }

        /// <summary>
        /// Única transición manual: Pendiente -> Pagada. Una cuota pagada solo vuelve a pendiente
        /// si se anula el ingreso que la pagó (<see cref="RevertirPago"/>).
        /// </summary>
        public void CambiarEstado(EstadoCuota nuevoEstado)
        {
            if (!Enum.IsDefined(nuevoEstado))
                throw new ReglaNegocioException("El estado de la cuota no es válido.");
            if (nuevoEstado == Estado)
                throw new ReglaNegocioException($"La cuota ya está {Estado.ToString().ToLowerInvariant()}.");
            if (EstaPagada)
                throw new ReglaNegocioException("Una cuota pagada no puede cambiar de estado.");

            Estado = nuevoEstado;
        }

        /// <summary>Vuelve la cuota a pendiente porque se anuló el ingreso con que se pagó.</summary>
        public void RevertirPago()
        {
            if (!EstaPagada)
                throw new ReglaNegocioException("La cuota no está pagada.");
            Estado = EstadoCuota.Pendiente;
        }
    }
}
