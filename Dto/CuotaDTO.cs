using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public class CuotaDTO
    {
        public int Id { get; set; }

        public int SocioId { get; set; }

        // "Apellido, Nombre (DNI)" del socio.
        public string Socio { get; set; } = string.Empty;

        public int Anio { get; set; }

        public decimal Monto { get; set; }

        public EstadoCuota Estado { get; set; }

        public string Descripcion
        {
            get { return "Cuota " + Anio + " - " + Formato.Monto(Monto); }
        }

        // Para elegir entre cuotas de varios socios (un responsable que paga por sus socios a cargo).
        public string DescripcionConSocio
        {
            get { return Descripcion + " - " + Socio; }
        }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
