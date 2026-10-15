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

        internal SocioService(ISocioRepository socios, IAuditoriaService auditoria)
        {
            _socios = socios;
            _auditoria = auditoria;
        }

        public SocioDto Registrar(SocioCrearDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // El propio Socio valida y normaliza sus datos; acá solo se controla lo que depende de otros socios.
            var socio = new Socio(dto.Nombre, dto.Apellido, dto.Dni, dto.Telefono, dto.Email, dto.Categoria);
            ExigirDniLibre(socio.Dni, socioId: null);

            // El propio Socio valida que el elegido sea un responsable.
            if (dto.ResponsableId is int responsableId)
                socio.AsignarResponsable(_socios.ObtenerExistente(responsableId));

            _socios.Agregar(socio);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Socio), socio.Id, socio.ObtenerDescripcion());

            return SocioMapper.ADto(socio);
        }

        public SocioDto Editar(SocioEditarDto dto, int usuarioId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var socio = _socios.ObtenerExistente(dto.Id);
            ExigirDniLibre((dto.Dni ?? string.Empty).Trim(), socio.Id);
            var responsable = dto.ResponsableId is int responsableId ? _socios.ObtenerExistente(responsableId) : null;

            var cambios = new DescripcionCambios();
            var anterior = SocioMapper.ADto(socio);

            // El Socio valida todo junto (incluida la categoría frente a sus socios a cargo) antes de modificar.
            socio.ActualizarDatos(dto.Nombre, dto.Apellido, dto.Dni ?? string.Empty, dto.Telefono, dto.Email,
                                  dto.Categoria, responsable, TieneSociosACargo(socio));
            _socios.Actualizar(socio);

            var nuevo = SocioMapper.ADto(socio);
            cambios.Campo("Nombre", anterior.Nombre, nuevo.Nombre)
                   .Campo("Apellido", anterior.Apellido, nuevo.Apellido)
                   .Campo("DNI", anterior.Dni, nuevo.Dni)
                   .Campo("Teléfono", anterior.Telefono, nuevo.Telefono)
                   .Campo("Email", anterior.Email, nuevo.Email)
                   .Campo("Categoría", anterior.Categoria, nuevo.Categoria)
                   .Campo("Responsable", anterior.Responsable, nuevo.Responsable);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Socio), socio.Id, cambios.ToString());

            return nuevo;
        }

        public SocioDto CambiarEstado(int socioId, bool activo, int usuarioId)
        {
            var socio = _socios.ObtenerExistente(socioId);

            if (activo)
                socio.Reactivar();
            else
                socio.DarDeBaja(TieneSociosACargo(socio));

            _socios.Actualizar(socio);

            // Dar de baja es la baja lógica del socio; reactivarlo es una modificación.
            var accion = activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja;
            _auditoria.Registrar(usuarioId, accion, nameof(Socio), socio.Id, activo ? "Reactivado" : "Dado de baja");

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

        private bool TieneSociosACargo(Socio socio) => _socios.ObtenerACargo(socio).Count > 0;

        /// <summary>El DNI no puede repetirse entre socios (el propio socio, al editarlo, no cuenta).</summary>
        private void ExigirDniLibre(string dni, int? socioId)
        {
            var existente = _socios.ObtenerPorDni(dni);
            if (existente is not null && existente.Id != socioId)
                throw new ReglaNegocioException("Ya existe un socio registrado con ese DNI.");
        }
    }
}
