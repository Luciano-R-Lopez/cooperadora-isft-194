using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    // Null (o texto vacío) significa "sin filtrar".
    public class AuditoriaFiltroDTO
    {
        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }

        public AccionAuditoria? Accion { get; set; }

        public string Texto { get; set; } = string.Empty;
    }
}
