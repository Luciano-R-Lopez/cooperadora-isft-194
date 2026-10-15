using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Common
{
    /// <summary>Singleton con el usuario logueado. Solo guarda el DTO, nunca la entidad.</summary>
    public sealed class SesionActual
    {
        private static readonly Lazy<SesionActual> _instancia = new(() => new SesionActual());
        public static SesionActual Instancia => _instancia.Value;

        private SesionActual() { }

        public UsuarioDto? Usuario { get; private set; }
        public bool HayUsuario => Usuario is not null;

        /// <summary>Se dispara cuando cambian los datos del usuario logueado (por ejemplo, si edita su nombre).</summary>
        public event Action<UsuarioDto>? UsuarioActualizado;

        public void Iniciar(UsuarioDto usuario) => Usuario = usuario;
        public void Cerrar() => Usuario = null;

        /// <summary>Refresca los datos guardados si el usuario editado es el logueado.</summary>
        public void Actualizar(UsuarioDto usuario)
        {
            if (Usuario is null || Usuario.Id != usuario.Id)
                return;

            Usuario = usuario;
            UsuarioActualizado?.Invoke(usuario);
        }

        public bool TieneRol(Rol rol) => Usuario?.Rol == rol;
    }
}
