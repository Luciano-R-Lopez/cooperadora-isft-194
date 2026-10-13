using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    public class CajaController : BaseController
    {
        private readonly ICajaService _cajas;

        public CajaController() : this(CajaService.Instancia, SesionActual.Instancia) { }

        public CajaController(ICajaService cajas, SesionActual sesion) : base(sesion)
        {
            _cajas = cajas;
        }

        /// <summary>Devuelve null cuando no hay ninguna caja abierta. Solo lectura: el Operador la consulta para saber si puede cobrar.</summary>
        public CajaDto? ObtenerActiva()
        {
            ExigirAlgunRol(Rol.Contador, Rol.Operador);
            return _cajas.ObtenerActiva();
        }

        public CajaDto Abrir(AbrirCajaDto dto)
        {
            ExigirRol(Rol.Contador);
            return _cajas.Abrir(dto, UsuarioActualId);
        }

        public ResumenCajaDto Cerrar(CerrarCajaDto dto)
        {
            ExigirRol(Rol.Contador);
            return _cajas.Cerrar(dto, UsuarioActualId);
        }

        public ResumenCajaDto ObtenerResumenActivo(decimal? montoDeclarado)
        {
            ExigirRol(Rol.Contador);
            return _cajas.ObtenerResumenActivo(montoDeclarado);
        }
    }
}
