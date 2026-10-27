using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Service.Interfaces
{
    public interface IAuthService
    {
        /// <exception cref="Common.Exceptions.AutenticacionException">Credenciales incorrectas o usuario sin acceso.</exception>
        UsuarioDto Autenticar(LoginDto login);
    }
}
