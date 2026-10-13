using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Auth;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class AuthService : IAuthService
    {
        private static readonly Lazy<AuthService> _instancia = new(() => new AuthService(UsuarioRepository.Instancia));
        public static AuthService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;

        private AuthService(IUsuarioRepository usuarios)
        {
            _usuarios = usuarios;
        }

        public UsuarioDto Autenticar(LoginDto login)
        {
            ArgumentNullException.ThrowIfNull(login);

            var nombreUsuario = login.NombreUsuario?.Trim() ?? string.Empty;
            var password = login.Password ?? string.Empty;

            if (nombreUsuario.Length == 0 || password.Length == 0)
                throw new AutenticacionException("Ingresá usuario y contraseña.");

            var usuario = _usuarios.ObtenerPorNombreUsuario(nombreUsuario);

            // Mismo mensaje si el usuario no existe o la contraseña es incorrecta,
            // para no revelar qué usuarios existen.
            if (usuario is null || !PasswordHasher.Verificar(password, usuario.PasswordHash))
                throw new AutenticacionException("Usuario o contraseña incorrectos.");

            if (!usuario.Activo)
                throw new AutenticacionException("El usuario está desactivado. Contactá a un administrador.");

            return UsuarioMapper.ADto(usuario);
        }
    }
}
