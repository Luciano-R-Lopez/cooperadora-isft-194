using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Auth;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    public class AuthController
    {
        private readonly IAuthService _auth;
        private readonly SesionActual _sesion;

        public AuthController() : this(AuthService.Instancia, SesionActual.Instancia) { }

        public AuthController(IAuthService auth, SesionActual sesion)
        {
            _auth = auth;
            _sesion = sesion;
        }

        /// <exception cref="Common.Exceptions.AutenticacionException">Credenciales incorrectas o usuario sin acceso.</exception>
        public UsuarioDto IniciarSesion(string nombreUsuario, string password)
        {
            var usuario = _auth.Autenticar(new LoginDto(nombreUsuario, password));
            _sesion.Iniciar(usuario);
            return usuario;
        }

        public void CerrarSesion() => _sesion.Cerrar();
    }
}
