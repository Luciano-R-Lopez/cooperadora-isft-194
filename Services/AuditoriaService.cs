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

        private AuditoriaService(IAuditoriaRepository auditorias, IUsuarioRepository usuarios)
        {
            _auditorias = auditorias;
            _usuarios = usuarios;
        }

        public void Registrar(int usuarioId, AccionAuditoria accion, string entidadAfectada, int entidadId)
        {
            var usuario = _usuarios.ObtenerExistente(usuarioId);
            _auditorias.Agregar(new Auditoria(usuario, accion, entidadAfectada, entidadId, DateTime.Now));
        }

        public IReadOnlyList<AuditoriaDto> ObtenerTodas() =>
            _auditorias.ObtenerTodos()
                       .OrderByDescending(a => a.Fecha)
                       .ThenByDescending(a => a.Id)
                       .Select(AuditoriaMapper.ADto)
                       .ToList();
    }
}
