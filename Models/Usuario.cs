using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Usuario : Persona
    {
        private string _passwordHash = string.Empty;
        private SortedSet<Rol> _roles;

        /// <param name="passwordHash">Hash PBKDF2: la contraseña en texto plano nunca llega al dominio.</param>
        /// <param name="roles">Uno o más roles: un usuario puede ser, por ejemplo, operador y contador a la vez.</param>
        public Usuario(string nombreUsuario, string nombre, string apellido, string passwordHash, bool activo,
                       params IEnumerable<Rol> roles)
            : base(nombre, apellido)
        {
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();
            if (!Validaciones.EsNombreUsuarioValido(nombreUsuario))
                throw new ReglaNegocioException(
                    "El nombre de usuario debe tener entre 3 y 30 caracteres: letras, números, punto, guion o guion bajo.");

            NombreUsuario = nombreUsuario;
            CambiarPasswordHash(passwordHash);
            Activo = activo;
            _roles = ValidarRoles(roles);
        }

        /// <summary>Identifica al usuario en el login; no se puede cambiar.</summary>
        public string NombreUsuario { get; private set; }

        public string PasswordHash => _passwordHash;

        /// <summary>Roles del usuario, sin repetir y en el orden del enum (Administrador, Operador, Contador).</summary>
        public IReadOnlyCollection<Rol> Roles => _roles;

        public bool Activo { get; private set; }

        public void CambiarPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ReglaNegocioException("La contraseña del usuario no puede estar vacía.");
            _passwordHash = passwordHash;
        }

        public void Activar() => Activo = true;

        public void Desactivar() => Activo = false;

        public bool TieneRol(Rol rol) => _roles.Contains(rol);

        /// <summary>
        /// Cambia nombre, apellido, estado y roles juntos: primero valida todo y recién después modifica,
        /// así si algún dato es inválido el usuario queda sin cambios.
        /// </summary>
        public void ActualizarDatos(string nombre, string apellido, bool activo, params IEnumerable<Rol> roles)
        {
            var nombreValido = Validaciones.ExigirTexto(nombre, "nombre", LargoMaximoNombre);
            var apellidoValido = Validaciones.ExigirTexto(apellido, "apellido", LargoMaximoNombre);
            var rolesValidos = ValidarRoles(roles);

            Nombre = nombreValido;
            Apellido = apellidoValido;
            Activo = activo;
            _roles = rolesValidos;
        }

        private static SortedSet<Rol> ValidarRoles(IEnumerable<Rol>? roles)
        {
            var validos = new SortedSet<Rol>(roles ?? Enumerable.Empty<Rol>());
            if (validos.Count == 0)
                throw new ReglaNegocioException("El usuario tiene que tener al menos un rol.");
            if (validos.Any(r => !Enum.IsDefined(r)))
                throw new ReglaNegocioException("El rol no es válido.");
            return validos;
        }
    }
}
