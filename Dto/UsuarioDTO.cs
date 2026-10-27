using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    // Se usa para mostrar, crear y editar usuarios. Password solo viaja de la pantalla al servicio
    // (al editar, vacía significa "no cambiar"); el servicio nunca la completa.
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string NombreUsuario { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        public List<Rol> Roles { get; set; } = new List<Rol>();

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }

        public string RolesTexto
        {
            get { return string.Join(", ", Roles); }
        }

        public string Estado
        {
            get { return Activo ? "Activo" : "Inactivo"; }
        }
    }
}
