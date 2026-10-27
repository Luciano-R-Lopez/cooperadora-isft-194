using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class MovimientoController : BaseController
    {
        private readonly MovimientoService servicio;

        public MovimientoController()
        {
            servicio = new MovimientoService();
        }

        public List<MovimientoDTO> Obtener(MovimientoFiltroDTO filtro)
        {
            ExigirRol(Rol.Contador);
            return servicio.Obtener(filtro);
        }

        public MovimientoDTO Anular(int movimientoId, string motivo)
        {
            ExigirRol(Rol.Contador);
            return servicio.Anular(movimientoId, motivo, UsuarioActualId);
        }

        public List<MovimientoDTO> ObtenerEgresos(DateTime desde, DateTime hasta)
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerEgresos(desde, hasta);
        }
    }
}
