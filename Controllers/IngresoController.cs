using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    public class IngresoController : BaseController
    {
        private readonly IIngresoService _ingresos;

        public IngresoController() : this(IngresoService.Instancia, SesionActual.Instancia) { }

        public IngresoController(IIngresoService ingresos, SesionActual sesion) : base(sesion)
        {
            _ingresos = ingresos;
        }

        public IngresoDto Registrar(IngresoCrearDto dto)
        {
            ExigirRol(Rol.Operador);
            return _ingresos.Registrar(dto, UsuarioActualId);
        }
    }
}
