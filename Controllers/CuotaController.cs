using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    public class CuotaController : BaseController
    {
        private readonly ICuotaService _cuotas;

        public CuotaController() : this(CuotaService.Instancia, SesionActual.Instancia) { }

        public CuotaController(ICuotaService cuotas, SesionActual sesion) : base(sesion)
        {
            _cuotas = cuotas;
        }

        public IReadOnlyList<CuotaDto> ObtenerPorSocio(int socioId)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.ObtenerPorSocio(socioId);
        }

        public CuotaDto Generar(CuotaCrearDto dto)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.Generar(dto, UsuarioActualId);
        }

        public decimal? ObtenerMontoSugerido(int anio)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.ObtenerMontoSugerido(anio);
        }

        public GeneracionCuotasDto GenerarParaTodos(int anio, decimal monto)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.GenerarParaTodos(anio, monto, UsuarioActualId);
        }

        public void FijarValorCuota(int anio, decimal monto)
        {
            ExigirRol(Rol.Operador);
            _cuotas.FijarValorCuota(anio, monto, UsuarioActualId);
        }

        /// <param name="pagadorId">El responsable del socio si es quien paga; null si paga el propio socio.</param>
        public ReciboPagoDto RegistrarPago(int cuotaId, int medioPagoId, int? pagadorId = null)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.RegistrarPago(cuotaId, medioPagoId, UsuarioActualId, pagadorId);
        }

        public IReadOnlyList<CuotaPendienteDto> ObtenerPendientes(int? anio)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.ObtenerPendientes(anio);
        }
    }
}
