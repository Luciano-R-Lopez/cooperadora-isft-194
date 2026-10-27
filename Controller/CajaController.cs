using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class CajaController : BaseController
    {
        private readonly CajaService servicio;

        public CajaController()
        {
            servicio = new CajaService();
        }

        // El operador también la consulta: sin caja abierta no puede cobrar.
        public CajaDTO? ObtenerActiva()
        {
            ExigirRol(Rol.Contador, Rol.Operador);
            return servicio.ObtenerActiva();
        }

        public List<CajaDTO> ObtenerTodas()
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerTodas();
        }

        public CajaDTO Abrir(decimal montoInicial)
        {
            ExigirRol(Rol.Contador);
            return servicio.Abrir(montoInicial, UsuarioActualId);
        }

        public ResumenCajaDTO Cerrar(decimal montoContado)
        {
            ExigirRol(Rol.Contador);
            return servicio.Cerrar(montoContado, UsuarioActualId);
        }

        public ResumenCajaDTO ObtenerResumenActivo(decimal? montoDeclarado)
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerResumenActivo(montoDeclarado);
        }

        public ResumenCajaDTO ObtenerResumen(int cajaId)
        {
            ExigirRol(Rol.Contador);
            return servicio.ObtenerResumen(cajaId);
        }
    }
}
