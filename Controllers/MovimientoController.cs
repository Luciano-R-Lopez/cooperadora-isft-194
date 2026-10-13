using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.DTOs.Movimientos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    /// <summary>Consulta de movimientos (dashboard). Solo para el Contador.</summary>
    public class MovimientoController : BaseController
    {
        private readonly IMovimientoService _movimientos;
        private readonly ICajaService _cajas;

        public MovimientoController()
            : this(MovimientoService.Instancia, CajaService.Instancia, SesionActual.Instancia) { }

        public MovimientoController(IMovimientoService movimientos, ICajaService cajas, SesionActual sesion) : base(sesion)
        {
            _movimientos = movimientos;
            _cajas = cajas;
        }

        public IReadOnlyList<CajaDto> ObtenerCajas()
        {
            ExigirRol(Rol.Contador);
            return _cajas.ObtenerTodas();
        }

        public IReadOnlyList<MovimientoDto> Obtener(MovimientoFiltroDto filtro)
        {
            ExigirRol(Rol.Contador);
            return _movimientos.Obtener(filtro);
        }
    }
}
