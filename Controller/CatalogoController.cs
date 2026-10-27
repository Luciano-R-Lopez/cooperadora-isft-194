using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    // Opciones de los desplegables y su mantenimiento por el administrador.
    public class CatalogoController : BaseController
    {
        private readonly CatalogoService servicio;

        public CatalogoController()
        {
            servicio = new CatalogoService();
        }

        public List<ItemCatalogoDTO> ObtenerMediosPago(bool incluirInactivos)
        {
            ExigirRol(Rol.Operador, Rol.Contador);
            return servicio.ObtenerItems(TipoCatalogo.MedioPago, incluirInactivos);
        }

        public List<ItemCatalogoDTO> ObtenerMotivosEgreso()
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerItems(TipoCatalogo.MotivoEgreso, false);
        }

        public List<ItemCatalogoDTO> ObtenerGruposEgreso(bool incluirInactivos)
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerItems(TipoCatalogo.GrupoEgreso, incluirInactivos);
        }

        public List<ItemCatalogoDTO> ObtenerItems(TipoCatalogo tipo)
        {
            ExigirRol(Rol.Administrador);
            return servicio.ObtenerItems(tipo, true);
        }

        public ItemCatalogoDTO Guardar(ItemCatalogoDTO dto)
        {
            ExigirRol(Rol.Administrador);
            return servicio.Guardar(dto, UsuarioActualId);
        }

        public ItemCatalogoDTO CambiarEstado(TipoCatalogo tipo, int id, bool activo)
        {
            ExigirRol(Rol.Administrador);
            return servicio.CambiarEstado(tipo, id, activo, UsuarioActualId);
        }
    }
}
