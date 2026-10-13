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

        /// <summary>Única transición permitida: Pendiente -> Pagada. Una cuota pagada no vuelve atrás.</summary>
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
    }
}
