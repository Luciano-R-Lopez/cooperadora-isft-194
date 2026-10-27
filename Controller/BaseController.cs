using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Controller
{
    /// <summary>Base de los controllers de cada módulo: centraliza el control de permisos por rol.</summary>
    public abstract class BaseController
    {
        protected SesionActual Sesion { get; }

        protected BaseController(SesionActual sesion)
        {
            Sesion = sesion;
        }

        /// <summary>Id del usuario logueado. Solo usar después de pasar por ExigirRol / ExigirAlgunRol.</summary>
        protected int UsuarioActualId => Sesion.Usuario?.Id ?? 0;

        /// <summary>Lanza AccesoDenegadoException si el usuario logueado no tiene el rol pedido.</summary>
        protected void ExigirRol(Rol rol) => ExigirAlgunRol(rol);

        /// <summary>Igual que ExigirRol, pero alcanza con tener cualquiera de los roles indicados.</summary>
        protected void ExigirAlgunRol(params Rol[] roles)
        {
            if (!roles.Any(Sesion.TieneRol))
                throw new AccesoDenegadoException();
        }
    }
}
