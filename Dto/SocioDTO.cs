using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    public class SocioDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Dni { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Categoria Categoria { get; set; }

        // Null si no tiene responsable.
        public int? ResponsableId { get; set; }

        public string Responsable { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        // "Apellido, Nombre (DNI)", para desplegables y títulos.
        public string Descripcion { get; set; } = string.Empty;

        public string Estado
        {
            get { return Activo ? "Activo" : "De baja"; }
        }

        public override string ToString()
        {
            return Descripcion;
        }
    }
}
