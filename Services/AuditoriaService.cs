using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class AuditoriaService : IAuditoriaService
    {
        private static readonly Lazy<AuditoriaService> _instancia = new(() =>
            new AuditoriaService(AuditoriaRepository.Instancia, UsuarioRepository.Instancia));
        public static AuditoriaService Instancia => _instancia.Value;

        private readonly IAuditoriaRepository _auditorias;
        private readonly IUsuarioRepository _usuarios;
        private readonly Func<DateTime> _reloj;

        /// <param name="reloj">De dónde sale la fecha y hora actual; los tests pasan una fija.</param>
        internal AuditoriaService(IAuditoriaRepository auditorias, IUsuarioRepository usuarios, Func<DateTime>? reloj = null)
        {
            _auditorias = auditorias;
            _usuarios = usuarios;
            _reloj = reloj ?? (() => DateTime.Now);
        }

        public void Registrar(int usuarioId, AccionAuditoria accion, string entidadAfectada, int entidadId, string? detalle = null)
        {
            var usuario = _usuarios.ObtenerExistente(usuarioId);
            _auditorias.Agregar(new Auditoria(usuario, accion, entidadAfectada, entidadId, _reloj(), detalle));
        }

        public IReadOnlyList<AuditoriaDto> Obtener(AuditoriaFiltroDto filtro)
        {
            ArgumentNullException.ThrowIfNull(filtro);
            if (filtro.Accion is AccionAuditoria accionElegida && !Enum.IsDefined(accionElegida))
                throw new ReglaNegocioException("La acción elegida no es válida.");
            if (filtro.Desde is DateTime d && filtro.Hasta is DateTime h && d.Date > h.Date)
                throw new ReglaNegocioException("La fecha \"desde\" no puede ser posterior a la fecha \"hasta\".");

            IEnumerable<Auditoria> registros = _auditorias.ObtenerTodos();

            if (filtro.Desde is DateTime desde)
                registros = registros.Where(a => a.Fecha.Date >= desde.Date);
            if (filtro.Hasta is DateTime hasta)
                registros = registros.Where(a => a.Fecha.Date <= hasta.Date);
            if (filtro.Accion is AccionAuditoria accion)
                registros = registros.Where(a => a.Accion == accion);
            if (!string.IsNullOrWhiteSpace(filtro.Texto))
            {
                var texto = filtro.Texto.Trim();
                registros = registros.Where(a =>
                    a.Usuario.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    a.EntidadAfectada.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    a.Detalle.Contains(texto, StringComparison.OrdinalIgnoreCase));
            }

            return registros
                       .OrderByDescending(a => a.Fecha)
                       .ThenByDescending(a => a.Id)
                       .Select(AuditoriaMapper.ADto)
                       .ToList();
        }
    }
}
