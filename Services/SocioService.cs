using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class SocioService : ISocioService
    {
        private static readonly Lazy<SocioService> _instancia = new(() =>
            new SocioService(SocioRepository.Instancia, AuditoriaService.Instancia));
        public static SocioService Instancia => _instancia.Value;

        private readonly ISocioRepository _socios;
        private readonly IAuditoriaService _auditoria;

        private SocioService(ISocioRepository socios, IAuditoriaService auditoria)
        {
            _socios = socios;
            _auditoria = auditoria;
        }

        public SocioDto Registrar(SocioCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // El propio Socio valida y normaliza sus datos; acá solo se controla lo que depende de otros socios.
            var socio = new Socio(dto.Nombre, dto.Apellido, dto.Dni, dto.Telefono, dto.Email, dto.Categoria);

            if (_socios.ObtenerPorDni(socio.Dni) is not null)
                throw new ReglaNegocioException("Ya existe un socio registrado con ese DNI.");

            // El propio Socio valida que el elegido sea un responsable.
            if (dto.ResponsableId is int responsableId)
                socio.AsignarResponsable(_socios.ObtenerExistente(responsableId));

            _socios.Agregar(socio);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Socio), socio.Id);

            return SocioMapper.ADto(socio);
        }

        public SocioDto ObtenerPorId(int id) =>
            SocioMapper.ADto(_socios.ObtenerExistente(id));

        public IReadOnlyList<SocioDto> Buscar(string? filtro) =>
            _socios.Buscar(filtro).Select(SocioMapper.ADto).ToList();

        public IReadOnlyList<SocioDto> ObtenerResponsables() =>
            _socios.ObtenerResponsables().Select(SocioMapper.ADto).ToList();

        public IReadOnlyList<SocioDto> ObtenerACargo(int responsableId)
        {
            var responsable = _socios.ObtenerExistente(responsableId);
            return _socios.ObtenerACargo(responsable).Select(SocioMapper.ADto).ToList();
        }
    }
}
