using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public class AuditoriaDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Usuario { get; set; } = string.Empty;

        public AccionAuditoria Accion { get; set; }

        public string EntidadAfectada { get; set; } = string.Empty;

        public int EntidadId { get; set; }

        public string Detalle { get; set; } = string.Empty;
    }
}
