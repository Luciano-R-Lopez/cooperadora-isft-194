using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
{
    public class EgresoController : BaseController
    {
        private readonly IEgresoService _egresos;

        public EgresoController() : this(EgresoService.Instancia, SesionActual.Instancia) { }

        public EgresoController(IEgresoService egresos, SesionActual sesion) : base(sesion)
        {
            _egresos = egresos;
        }

        public EgresoDto Registrar(EgresoCrearDto dto)
        {
            ExigirRol(Rol.Contador);
            return _egresos.Registrar(dto, UsuarioActualId);
        }
    }
}
