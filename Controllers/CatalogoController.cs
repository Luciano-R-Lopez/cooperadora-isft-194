using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    /// <summary>Opciones de los desplegables (medios de pago, motivos de egreso) para los módulos que registran movimientos.</summary>
    public class CatalogoController : BaseController
    {
        private readonly ICatalogoService _catalogos;

        public CatalogoController() : this(CatalogoService.Instancia, SesionActual.Instancia) { }

        public CatalogoController(ICatalogoService catalogos, SesionActual sesion) : base(sesion)
        {
            _catalogos = catalogos;
        }

        public IReadOnlyList<MedioPagoDto> ObtenerMediosPago()
        {
            ExigirAlgunRol(Rol.Operador, Rol.Contador, Rol.Administrador);
            return _catalogos.ObtenerMediosPago();
        }

        public IReadOnlyList<MotivoEgresoDto> ObtenerMotivosEgreso()
        {
            ExigirRol(Rol.Contador);
            return _catalogos.ObtenerMotivosEgreso();
        }
    }
}
