using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Usuario : Persona
    {
        private string _passwordHash = string.Empty;
        private Rol _rol;

        /// <param name="passwordHash">Hash PBKDF2: la contraseña en texto plano nunca llega al dominio.</param>
        public Usuario(string nombreUsuario, string nombre, string apellido, string passwordHash, bool activo, Rol rol)
            : base(nombre, apellido)
        {
            nombreUsuario = (nombreUsuario ?? string.Empty).Trim();
            if (!Validaciones.EsNombreUsuarioValido(nombreUsuario))
                throw new ReglaNegocioException(
                    "El nombre de usuario debe tener entre 3 y 30 caracteres: letras, números, punto, guion o guion bajo.");

            NombreUsuario = nombreUsuario;
            CambiarPasswordHash(passwordHash);
            Activo = activo;
            Rol = rol;
        }

        /// <summary>Identifica al usuario en el login; no se puede cambiar.</summary>
        public string NombreUsuario { get; private set; }

        public string PasswordHash => _passwordHash;

        public Rol Rol
        {
            get => _rol;
            private set
            {
                if (!Enum.IsDefined(value))
                    throw new ReglaNegocioException("El rol no es válido.");
                _rol = value;
            }
        }

        public bool Activo { get; private set; }

        public void CambiarPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ReglaNegocioException("La contraseña del usuario no puede estar vacía.");
            _passwordHash = passwordHash;
        }

        public void Activar() => Activo = true;

        public void Desactivar() => Activo = false;

        public bool TieneRol(Rol rol) => Rol == rol;

        /// <summary>
        /// Cambia nombre, apellido, estado y rol juntos: primero valida todo y recién después modifica,
        /// así si algún dato es inválido el usuario queda sin cambios.
        /// </summary>
        public void ActualizarDatos(string nombre, string apellido, bool activo, Rol rol)
        {
            var nombreValido = Validaciones.ExigirTexto(nombre, "nombre", LargoMaximoNombre);
            var apellidoValido = Validaciones.ExigirTexto(apellido, "apellido", LargoMaximoNombre);
            if (!Enum.IsDefined(rol))
                throw new ReglaNegocioException("El rol no es válido.");

            Nombre = nombreValido;
            Apellido = apellidoValido;
            Activo = activo;
            Rol = rol;
        }
    }
}
