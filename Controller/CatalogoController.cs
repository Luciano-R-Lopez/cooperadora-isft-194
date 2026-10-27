using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
{
    /// <summary>Opciones de los desplegables (medios de pago, motivos y grupos de egreso) y su mantenimiento por el administrador.</summary>
    public class CatalogoController : BaseController
    {
        private readonly ICatalogoService _catalogos;

        public CatalogoController() : this(CatalogoService.Instancia, SesionActual.Instancia) { }

        public CatalogoController(ICatalogoService catalogos, SesionActual sesion) : base(sesion)
        {
            _catalogos = catalogos;
        }

        public IReadOnlyList<MedioPagoDto> ObtenerMediosPago(bool incluirInactivos = false)
        {
            ExigirAlgunRol(Rol.Operador, Rol.Contador);
            return _catalogos.ObtenerMediosPago(incluirInactivos);
        }

        public IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso()
        {
            ExigirRol(Rol.Contador);
            return _catalogos.ObtenerMotivosEgreso();
        }

        /// <param name="incluirInactivos">True para filtrar movimientos ya registrados.</param>
        public IReadOnlyList<GrupoEgresoDto> ObtenerGruposEgreso(bool incluirInactivos = false)
        {
            ExigirRol(Rol.Contador);
            return _catalogos.ObtenerGruposEgreso(incluirInactivos);
        }

        public IReadOnlyList<ItemCatalogoDto> ObtenerItems(TipoCatalogo tipo)
        {
            ExigirRol(Rol.Administrador);
            return _catalogos.ObtenerItems(tipo);
        }

        public ItemCatalogoDto Agregar(TipoCatalogo tipo, string nombre)
        {
            ExigirRol(Rol.Administrador);
            return _catalogos.Agregar(tipo, nombre, UsuarioActualId);
        }

        public ItemCatalogoDto Renombrar(TipoCatalogo tipo, int id, string nombre)
        {
            ExigirRol(Rol.Administrador);
            return _catalogos.Renombrar(tipo, id, nombre, UsuarioActualId);
        }

        public ItemCatalogoDto CambiarEstado(TipoCatalogo tipo, int id, bool activo)
        {
            ExigirRol(Rol.Administrador);
            return _catalogos.CambiarEstado(tipo, id, activo, UsuarioActualId);
        }
    }
}
