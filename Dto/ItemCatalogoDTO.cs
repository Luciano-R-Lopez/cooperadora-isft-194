using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    // Medio de pago, motivo o grupo de egreso: para la pantalla de catálogos y para los desplegables.
    public class ItemCatalogoDTO
    {
        public int Id { get; set; }

        public TipoCatalogo Tipo { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public string Estado
        {
            get { return Activo ? "Activo" : "Inactivo"; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
