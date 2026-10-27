using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Service;
using CooperadoraISFT94.Service.Interfaces;

namespace CooperadoraISFT94.Controller
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

        public SocioDto Editar(SocioEditarDto dto)
        {
            ExigirRol(Rol.Operador);
            return _socios.Editar(dto, UsuarioActualId);
        }

        public SocioDto CambiarEstado(int socioId, bool activo)
        {
            ExigirRol(Rol.Operador);
            return _socios.CambiarEstado(socioId, activo, UsuarioActualId);
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
