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

        public CuotaDto RegistrarPago(int cuotaId, int medioPagoId)
        {
            ExigirRol(Rol.Operador);
            return _cuotas.RegistrarPago(cuotaId, medioPagoId, UsuarioActualId);
        }
    }
}
