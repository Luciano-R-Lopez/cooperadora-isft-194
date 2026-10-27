using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Controller
{
    // Controla los permisos: cada acción pide uno o más roles y alcanza con que el usuario tenga alguno.
    public abstract class BaseController
    {
        protected SesionActual Sesion
        {
            get { return SesionActual.Instancia; }
        }

        // Id del usuario que inició sesión. Se usa después de ExigirRol.
        protected int UsuarioActualId
        {
            get { return Sesion.Usuario?.Id ?? 0; }
        }

        protected void ExigirRol(params Rol[] roles)
        {
            if (!roles.Any(Sesion.TieneRol))
            {
                throw new AccesoDenegadoException();
            }
        }
    }
}
