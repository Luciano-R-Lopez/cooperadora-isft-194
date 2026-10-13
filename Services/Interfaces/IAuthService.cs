using CooperadoraISFT94.DTOs.Auth;
using CooperadoraISFT94.DTOs.Usuarios;

namespace CooperadoraISFT94.Services.Interfaces
{
    public interface IAuthService
    {
        /// <exception cref="Common.Exceptions.AutenticacionException">Credenciales incorrectas o usuario sin acceso.</exception>
        UsuarioDto Autenticar(LoginDto login);
    }
}
