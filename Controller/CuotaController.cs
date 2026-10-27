using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Service;

namespace CooperadoraISFT94.Controller
{
    public class CuotaController : BaseController
    {
        private readonly CuotaService servicio;

        public CuotaController()
        {
            servicio = new CuotaService();
        }

        public List<CuotaDTO> ObtenerPorSocio(int socioId)
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerPorSocio(socioId);
        }

        public CuotaDTO Generar(CuotaDTO dto)
        {
            ExigirRol(Rol.Operador);
            return servicio.Generar(dto, UsuarioActualId);
        }

        public GeneracionCuotasDTO GenerarParaTodos(int anio, decimal monto)
        {
            ExigirRol(Rol.Operador);
            return servicio.GenerarParaTodos(anio, monto, UsuarioActualId);
        }

        public decimal? ObtenerMontoSugerido(int anio)
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerMontoSugerido(anio);
        }

        public void FijarValorCuota(int anio, decimal monto)
        {
            ExigirRol(Rol.Operador);
            servicio.FijarValorCuota(anio, monto, UsuarioActualId);
        }

        // pagadorId: el responsable del socio si es quien paga; null si paga el propio socio.
        public ReciboPagoDTO RegistrarPagos(List<int> cuotaIds, int medioPagoId, int? pagadorId)
        {
            ExigirRol(Rol.Operador);
            return servicio.RegistrarPagos(cuotaIds, medioPagoId, UsuarioActualId, pagadorId);
        }

        public List<DeudorDTO> ObtenerDeudores(DeudoresFiltroDTO filtro)
        {
            ExigirRol(Rol.Operador);
            return servicio.ObtenerDeudores(filtro);
        }
    }
}
