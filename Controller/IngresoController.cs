using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
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
