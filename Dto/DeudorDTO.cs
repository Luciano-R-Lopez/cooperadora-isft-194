namespace CooperadoraISFT94.Dto
{
    // Fila de la lista de morosos: un socio con sus cuotas pendientes y quién las paga.
    public class DeudorDTO
    {
        public int SocioId { get; set; }

        public string Socio { get; set; } = string.Empty;

        public string Dni { get; set; } = string.Empty;

        // "El socio" o "Su responsable: ...".
        public string Paga { get; set; } = string.Empty;

        public string AniosQueDebe { get; set; } = string.Empty;

        public int CantidadCuotas { get; set; }

        public decimal Total { get; set; }

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // El email al que conviene escribir: el del responsable si es quien paga, si no el del socio.
        public string EmailDeContacto { get; set; } = string.Empty;

        public int? ResponsableId { get; set; }

        // "Apellido, Nombre (DNI)" del responsable; vacío si no tiene.
        public string Responsable { get; set; } = string.Empty;

        public bool SocioActivo { get; set; }

        public string Estado
        {
            get { return SocioActivo ? "Activo" : "De baja"; }
        }
    }
}
