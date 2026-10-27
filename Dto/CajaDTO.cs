using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public class CajaDTO
    {
        public int Id { get; set; }

        public DateTime FechaApertura { get; set; }

        public DateTime? FechaCierre { get; set; }

        public decimal MontoInicial { get; set; }

        public decimal MontoTotal { get; set; }

        public EstadoCaja Estado { get; set; }

        public bool Abierta
        {
            get { return Estado == EstadoCaja.Abierta; }
        }

        public string Descripcion
        {
            get { return "Caja #" + Id + " - " + FechaApertura.ToString(Formato.FechaHora) + " (" + Estado.ToString().ToUpperInvariant() + ")"; }
        }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
