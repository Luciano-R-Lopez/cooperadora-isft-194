using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class AuthService
    {
        // Los intentos fallidos se recuerdan mientras la aplicación está abierta, aunque se cree otro servicio.
        private static readonly Dictionary<string, IntentosLogin> intentosCompartidos = new Dictionary<string, IntentosLogin>();

        private readonly IRepository<Usuario> repositorio;
        private readonly Dictionary<string, IntentosLogin> intentos;
        private readonly Func<DateTime> reloj;

        public AuthService()
            : this(Repositorios.Usuarios, null, intentosCompartidos)
        {
        }

        // Los tests pasan un reloj fijo y sus propios intentos.
        public AuthService(IRepository<Usuario> repositorio, Func<DateTime>? reloj = null,
            Dictionary<string, IntentosLogin>? intentos = null)
        {
            this.repositorio = repositorio;
            this.reloj = reloj ?? (() => DateTime.Now);
            this.intentos = intentos ?? new Dictionary<string, IntentosLogin>();
        }

        public UsuarioDTO Autenticar(LoginDTO login)
        {
            string nombreUsuario = (login.NombreUsuario ?? string.Empty).Trim();
            string password = login.Password ?? string.Empty;
            if (nombreUsuario.Length == 0 || password.Length == 0)
            {
                throw new AutenticacionException("Ingrese usuario y contraseña.");
            }

            // Se cuentan también los nombres que no existen, para no revelar qué usuarios existen.
            string clave = nombreUsuario.ToLowerInvariant();
            ExigirNoBloqueado(clave);

            Usuario? usuario = repositorio.ObtenerTodos().FirstOrDefault(u =>
                string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase));
            if (usuario == null || !PasswordHasher.Verificar(password, usuario.PasswordHash))
            {
                RegistrarFallo(clave);
                throw new AutenticacionException("Usuario o contraseña incorrectos.");
            }

            intentos.Remove(clave);
            if (!usuario.Activo)
            {
                throw new AutenticacionException("El usuario está desactivado. Consulte con un administrador.");
            }
            return UsuarioService.MapearADto(usuario);
        }

        private void ExigirNoBloqueado(string clave)
        {
            if (!intentos.TryGetValue(clave, out IntentosLogin? registro) || registro.BloqueadoHasta == null)
            {
                return;
            }

            TimeSpan restante = registro.BloqueadoHasta.Value - reloj();
            if (restante <= TimeSpan.Zero)
            {
                // El bloqueo venció: se empieza a contar de nuevo.
                intentos.Remove(clave);
                return;
            }

            int minutos = (int)Math.Ceiling(restante.TotalMinutes);
            throw new AutenticacionException("Demasiados intentos fallidos. Pruebe de nuevo en " + minutos
                + (minutos == 1 ? " minuto." : " minutos."));
        }

        private void RegistrarFallo(string clave)
        {
            IntentosLogin registro = intentos.TryGetValue(clave, out IntentosLogin? existente) ? existente : new IntentosLogin();
            registro.Fallos++;
            intentos[clave] = registro;

            if (registro.Fallos >= Configuracion.IntentosLoginPermitidos)
            {
                registro.BloqueadoHasta = reloj().AddMinutes(Configuracion.MinutosBloqueoLogin);
                throw new AutenticacionException("Demasiados intentos fallidos. El ingreso con ese usuario quedó bloqueado por "
                    + Configuracion.MinutosBloqueoLogin + " minutos.");
            }
        }
    }

    public class IntentosLogin
    {
        public int Fallos { get; set; }

        public DateTime? BloqueadoHasta { get; set; }
    }
}
