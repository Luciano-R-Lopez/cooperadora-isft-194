using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class AuditoriaService
    {
        private readonly IRepository<Auditoria> repositorio;
        private readonly IRepository<Usuario> usuarioRepositorio;
        private readonly Func<DateTime> reloj;

        public AuditoriaService()
            : this(Repositorios.Auditorias, Repositorios.Usuarios)
        {
        }

        // reloj: de dónde sale la fecha y hora actual; los tests pasan una fija.
        public AuditoriaService(IRepository<Auditoria> repositorio, IRepository<Usuario> usuarioRepositorio,
            Func<DateTime>? reloj = null)
        {
            this.repositorio = repositorio;
            this.usuarioRepositorio = usuarioRepositorio;
            this.reloj = reloj ?? (() => DateTime.Now);
        }

        public void Registrar(int usuarioId, AccionAuditoria accion, string entidadAfectada, int entidadId, string detalle)
        {
            Usuario usuario = usuarioRepositorio.ObtenerExistente(usuarioId);
            Auditoria registro = new Auditoria(repositorio.ObtenerProximoId(), usuario, accion, entidadAfectada, entidadId,
                reloj(), detalle);
            repositorio.Agregar(registro);
        }

        public List<AuditoriaDTO> Obtener(AuditoriaFiltroDTO filtro)
        {
            if (filtro == null)
            {
                throw new ArgumentException("El filtro es obligatorio.");
            }
            if (filtro.Desde.HasValue && filtro.Hasta.HasValue && filtro.Desde.Value.Date > filtro.Hasta.Value.Date)
            {
                throw new NegocioException("La fecha \"desde\" no puede ser posterior a la fecha \"hasta\".");
            }

            IEnumerable<Auditoria> registros = repositorio.ObtenerTodos();
            if (filtro.Desde.HasValue)
            {
                registros = registros.Where(a => a.Fecha.Date >= filtro.Desde.Value.Date);
            }
            if (filtro.Hasta.HasValue)
            {
                registros = registros.Where(a => a.Fecha.Date <= filtro.Hasta.Value.Date);
            }
            if (filtro.Accion.HasValue)
            {
                registros = registros.Where(a => a.Accion == filtro.Accion.Value);
            }

            return registros.OrderByDescending(a => a.Fecha)
                            .ThenByDescending(a => a.Id)
                            .Select(MapearADto)
                            .ToList();
        }

        private static AuditoriaDTO MapearADto(Auditoria registro)
        {
            AuditoriaDTO dto = new AuditoriaDTO();
            dto.Id = registro.Id;
            dto.Fecha = registro.Fecha;
            dto.Usuario = registro.Usuario.NombreCompleto;
            dto.Accion = registro.Accion;
            dto.EntidadAfectada = registro.EntidadAfectada;
            dto.EntidadId = registro.EntidadId;
            dto.Detalle = registro.Detalle;
            return dto;
        }
    }
}
