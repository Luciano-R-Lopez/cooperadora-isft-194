using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Common
{
    // Usuario que inició sesión. Guarda el DTO, nunca la entidad.
    public class SesionActual
    {
        private static readonly SesionActual instancia = new SesionActual();

        private SesionActual()
        {
        }

        public static SesionActual Instancia
        {
            get { return instancia; }
        }

        public UsuarioDTO? Usuario { get; private set; }

        // Se dispara cuando cambian los datos del usuario que inició sesión (por ejemplo, si edita su nombre).
        public event Action<UsuarioDTO>? UsuarioActualizado;

        public void Iniciar(UsuarioDTO usuario)
        {
            Usuario = usuario;
        }

        public void Cerrar()
        {
            Usuario = null;
        }

        public void Actualizar(UsuarioDTO usuario)
        {
            if (Usuario == null || Usuario.Id != usuario.Id)
            {
                return;
            }
            Usuario = usuario;
            UsuarioActualizado?.Invoke(usuario);
        }

        public bool TieneRol(Rol rol)
        {
            return Usuario != null && Usuario.Roles.Contains(rol);
        }
    }
}
