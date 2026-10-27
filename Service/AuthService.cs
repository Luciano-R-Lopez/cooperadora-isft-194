using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository.Memoria;
using CooperadoraISFT94.Repository;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Service
{
    public sealed class AuthService : IAuthService
    {
        /// <summary>Intentos fallidos seguidos que bloquean el ingreso con ese nombre de usuario.</summary>
        public const int IntentosPermitidos = 5;

        /// <summary>Cuánto dura el bloqueo.</summary>
        public static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(5);

        private static readonly Lazy<AuthService> _instancia = new(() => new AuthService(UsuarioRepository.Instancia));
        public static AuthService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly Func<DateTime> _reloj;

        /// <summary>
        /// Intentos fallidos por nombre de usuario (en minúsculas). Se cuentan también los nombres que no existen,
        /// para que el bloqueo no revele qué usuarios existen.
        /// </summary>
        private readonly Dictionary<string, Intentos> _intentos = new();

        internal AuthService(IUsuarioRepository usuarios, Func<DateTime>? reloj = null)
        {
            _usuarios = usuarios;
            _reloj = reloj ?? (() => DateTime.Now);
        }

        public UsuarioDto Autenticar(LoginDto login)
        {
            ArgumentNullException.ThrowIfNull(login);

            var nombreUsuario = login.NombreUsuario?.Trim() ?? string.Empty;
            var password = login.Password ?? string.Empty;

            if (nombreUsuario.Length == 0 || password.Length == 0)
                throw new AutenticacionException("Ingresá usuario y contraseña.");

            var clave = nombreUsuario.ToLowerInvariant();
            ExigirNoBloqueado(clave);

            var usuario = _usuarios.ObtenerPorNombreUsuario(nombreUsuario);

            // Mismo mensaje si el usuario no existe o la contraseña es incorrecta,
            // para no revelar qué usuarios existen.
            if (usuario is null || !PasswordHasher.Verificar(password, usuario.PasswordHash))
            {
                RegistrarFallo(clave);
                throw new AutenticacionException("Usuario o contraseña incorrectos.");
            }

            _intentos.Remove(clave);

            if (!usuario.Activo)
                throw new AutenticacionException("El usuario está desactivado. Contactá a un administrador.");

            return UsuarioMapper.ADto(usuario);
        }

        private void ExigirNoBloqueado(string clave)
        {
            if (!_intentos.TryGetValue(clave, out var intentos) || intentos.BloqueadoHasta is not DateTime hasta)
                return;

            var restante = hasta - _reloj();
            if (restante <= TimeSpan.Zero)
            {
                // El bloqueo venció: se empieza a contar de nuevo.
                _intentos.Remove(clave);
                return;
            }

            var minutos = (int)Math.Ceiling(restante.TotalMinutes);
            throw new AutenticacionException(
                $"Demasiados intentos fallidos. Probá de nuevo en {minutos} minuto{(minutos == 1 ? "" : "s")}.");
        }

        private void RegistrarFallo(string clave)
        {
            var fallos = (_intentos.TryGetValue(clave, out var intentos) ? intentos.Fallos : 0) + 1;

            if (fallos >= IntentosPermitidos)
            {
                _intentos[clave] = new Intentos(fallos, _reloj() + DuracionBloqueo);
                throw new AutenticacionException(
                    $"Demasiados intentos fallidos. El ingreso con ese usuario quedó bloqueado por {DuracionBloqueo.TotalMinutes:0} minutos.");
            }

            _intentos[clave] = new Intentos(fallos, null);
        }

        private sealed record Intentos(int Fallos, DateTime? BloqueadoHasta);
    }
}
