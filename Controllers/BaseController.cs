using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Controllers
{
    /// <summary>Base de los controllers de cada módulo: centraliza el control de permisos por rol.</summary>
    public abstract class BaseController
    {
        private readonly SesionActual _sesion;

        protected BaseController(SesionActual sesion)
        {
            _sesion = sesion;
        }

        /// <summary>Id del usuario logueado. Solo usar después de pasar por ExigirRol / ExigirAlgunRol.</summary>
        protected int UsuarioActualId => _sesion.Usuario?.Id ?? 0;

        /// <summary>Lanza AccesoDenegadoException si el usuario logueado no tiene el rol pedido.</summary>
        protected void ExigirRol(Rol rol) => ExigirAlgunRol(rol);

        /// <summary>Igual que ExigirRol, pero alcanza con tener cualquiera de los roles indicados.</summary>
        protected void ExigirAlgunRol(params Rol[] roles)
        {
            if (!roles.Any(_sesion.TieneRol))
                throw new AccesoDenegadoException();
        }
    }
}
