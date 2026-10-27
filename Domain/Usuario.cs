using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Usuario : Persona
    {
        private string nombreUsuario = string.Empty;
        private string passwordHash = string.Empty;
        private SortedSet<Rol> roles = new SortedSet<Rol>();

        // La contraseña llega convertida en hash: en texto plano nunca llega al dominio.
        // Un usuario puede tener varios roles (por ejemplo, operador y contador a la vez).
        public Usuario(int id, string nombreUsuario, string nombre, string apellido, string passwordHash, bool activo,
            params IEnumerable<Rol> roles)
            : base(id, nombre, apellido)
        {
            NombreUsuario = nombreUsuario;
            PasswordHash = passwordHash;
            Activo = activo;
            AsignarRoles(roles);
        }

        // Identifica al usuario en el inicio de sesión; no se puede cambiar.
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            private set
            {
                string texto = (value ?? string.Empty).Trim();
                if (!Validaciones.EsNombreUsuarioValido(texto))
                {
                    throw new NegocioException("El nombre de usuario debe tener entre 3 y 30 caracteres: letras, números, punto, guion o guion bajo.");
                }
                nombreUsuario = texto;
            }
        }

        public string PasswordHash
        {
            get { return passwordHash; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new NegocioException("La contraseña del usuario no puede estar vacía.");
                }
                passwordHash = value;
            }
        }

        public bool Activo { get; set; }

        // Sin repetir y en el orden del enum (Administrador, Operador, Contador).
        public IReadOnlyCollection<Rol> Roles
        {
            get { return roles; }
        }

        public void AsignarRoles(IEnumerable<Rol>? nuevosRoles)
        {
            SortedSet<Rol> validos = new SortedSet<Rol>(nuevosRoles ?? Enumerable.Empty<Rol>());
            if (validos.Count == 0)
            {
                throw new NegocioException("El usuario tiene que tener al menos un rol.");
            }
            if (validos.Any(rol => !Enum.IsDefined(rol)))
            {
                throw new NegocioException("El rol no es válido.");
            }
            roles = validos;
        }

        public bool TieneRol(Rol rol)
        {
            return roles.Contains(rol);
        }
    }
}
