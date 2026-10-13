using CooperadoraISFT94.Common;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Services;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Controllers
{
    public class SocioController : BaseController
    {
        private readonly ISocioService _socios;

        public SocioController() : this(SocioService.Instancia, SesionActual.Instancia) { }

        public SocioController(ISocioService socios, SesionActual sesion) : base(sesion)
        {
            _socios = socios;
        }

        public SocioDto Registrar(SocioCrearDto dto)
        {
            ExigirRol(Rol.Operador);
            return _socios.Registrar(dto, UsuarioActualId);
        }

        public SocioDto ObtenerPorId(int id)
        {
            ExigirRol(Rol.Operador);
            return _socios.ObtenerPorId(id);
        }

        public IReadOnlyList<SocioDto> ObtenerResponsables()
        {
            ExigirRol(Rol.Operador);
            return _socios.ObtenerResponsables();
        }

        public IReadOnlyList<SocioDto> ObtenerACargo(int responsableId)
        {
            ExigirRol(Rol.Operador);
            return _socios.ObtenerACargo(responsableId);
        }

        public IReadOnlyList<SocioDto> Buscar(string? filtro)
        {
            ExigirRol(Rol.Operador);
            return _socios.Buscar(filtro);
        }
    }
}
