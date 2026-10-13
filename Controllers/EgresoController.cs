using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
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
