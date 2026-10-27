using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class AuthController
    {
        private readonly AuthService servicio;

        public AuthController()
        {
            servicio = new AuthService();
        }

        public UsuarioDTO IniciarSesion(LoginDTO login)
        {
            UsuarioDTO usuario = servicio.Autenticar(login);
            SesionActual.Instancia.Iniciar(usuario);
            return usuario;
        }

        public void CerrarSesion()
        {
            SesionActual.Instancia.Cerrar();
        }
    }
}
