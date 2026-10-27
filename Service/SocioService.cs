using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class SocioService
    {
        private readonly IRepository<Socio> repositorio;
        private readonly AuditoriaService auditoria;

        public SocioService()
            : this(Repositorios.Socios, new AuditoriaService())
        {
        }

        public SocioService(IRepository<Socio> repositorio, AuditoriaService auditoria)
        {
            this.repositorio = repositorio;
            this.auditoria = auditoria;
        }

        // Crea el socio si dto.Id es 0; si no, lo edita. Devuelve el id del socio guardado.
        public int Guardar(SocioDTO dto, int usuarioId)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos del socio son obligatorios.");
            }
            return dto.Id == 0 ? Crear(dto, usuarioId) : Editar(dto, usuarioId);
        }

        private int Crear(SocioDTO dto, int usuarioId)
        {
            Socio nuevo = new Socio(repositorio.ObtenerProximoId(), dto.Nombre, dto.Apellido, dto.Dni, dto.Telefono,
                dto.Email, dto.Categoria);
            ExigirDniLibre(nuevo.Dni, 0);
            nuevo.AsignarResponsable(BuscarResponsable(dto.ResponsableId));

            repositorio.Agregar(nuevo);
            auditoria.Registrar(usuarioId, AccionAuditoria.Alta, nameof(Socio), nuevo.Id, nuevo.ObtenerDescripcion());
            return nuevo.Id;
        }

        private int Editar(SocioDTO dto, int usuarioId)
        {
            Socio socio = repositorio.ObtenerExistente(dto.Id);

            // Se validan todos los datos con un socio aparte antes de tocar el guardado.
            Socio validado = new Socio(socio.Id, dto.Nombre, dto.Apellido, dto.Dni, dto.Telefono, dto.Email, dto.Categoria);
            Socio? responsable = BuscarResponsable(dto.ResponsableId);
            validado.AsignarResponsable(responsable);
            ExigirDniLibre(validado.Dni, socio.Id);
            if (TieneSociosACargo(socio) && !validado.EsResponsable)
            {
                throw new NegocioException("El socio tiene socios a cargo: reasígnelos antes de quitarle la categoría Responsable.");
            }

            SocioDTO anterior = MapearADto(socio);
            socio.Nombre = validado.Nombre;
            socio.Apellido = validado.Apellido;
            socio.Dni = validado.Dni;
            socio.Telefono = validado.Telefono;
            socio.Email = validado.Email;
            socio.Categoria = validado.Categoria;
            socio.AsignarResponsable(responsable);
            repositorio.Actualizar(socio);

            SocioDTO nuevo = MapearADto(socio);
            DescripcionCambios cambios = new DescripcionCambios()
                .Campo("Nombre", anterior.Nombre, nuevo.Nombre)
                .Campo("Apellido", anterior.Apellido, nuevo.Apellido)
                .Campo("DNI", anterior.Dni, nuevo.Dni)
                .Campo("Teléfono", anterior.Telefono, nuevo.Telefono)
                .Campo("Email", anterior.Email, nuevo.Email)
                .Campo("Categoría", anterior.Categoria, nuevo.Categoria)
                .Campo("Responsable", anterior.Responsable, nuevo.Responsable);
            auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Socio), socio.Id, cambios.ToString());
            return socio.Id;
        }

        // Dar de baja es la baja lógica: el socio conserva sus cuotas y pagos.
        public SocioDTO CambiarEstado(int socioId, bool activo, int usuarioId)
        {
            Socio socio = repositorio.ObtenerExistente(socioId);
            if (activo)
            {
                socio.Reactivar();
            }
            else
            {
                if (TieneSociosACargo(socio))
                {
                    throw new NegocioException("El socio tiene socios a cargo: reasígnelos antes de darlo de baja.");
                }
                socio.DarDeBaja();
            }
            repositorio.Actualizar(socio);

            auditoria.Registrar(usuarioId, activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja, nameof(Socio),
                socio.Id, activo ? "Reactivado" : "Dado de baja");
            return MapearADto(socio);
        }

        public List<SocioDTO> ObtenerTodos()
        {
            return Ordenar(repositorio.ObtenerTodos()).Select(MapearADto).ToList();
        }

        public SocioDTO ObtenerPorId(int id)
        {
            return MapearADto(repositorio.ObtenerExistente(id));
        }

        // Socios activos de categoría Responsable: los que se pueden elegir como responsables.
        public List<SocioDTO> ObtenerResponsables()
        {
            return Ordenar(repositorio.ObtenerTodos().Where(s => s.EsResponsable && s.Activo)).Select(MapearADto).ToList();
        }

        public List<SocioDTO> ObtenerACargo(int responsableId)
        {
            Socio responsable = repositorio.ObtenerExistente(responsableId);
            return Ordenar(SociosACargo(responsable)).Select(MapearADto).ToList();
        }

        private Socio? BuscarResponsable(int? responsableId)
        {
            return responsableId.HasValue ? repositorio.ObtenerExistente(responsableId.Value) : null;
        }

        private IEnumerable<Socio> SociosACargo(Socio responsable)
        {
            return repositorio.ObtenerTodos().Where(s => s.Responsable != null && s.Responsable.Id == responsable.Id);
        }

        private bool TieneSociosACargo(Socio socio)
        {
            return SociosACargo(socio).Any();
        }

        private void ExigirDniLibre(string dni, int idAIgnorar)
        {
            if (repositorio.ObtenerTodos().Any(s => s.Id != idAIgnorar && s.Dni == dni))
            {
                throw new NegocioException("Ya existe un socio registrado con ese DNI.");
            }
        }

        private static IEnumerable<Socio> Ordenar(IEnumerable<Socio> socios)
        {
            return socios.OrderBy(s => s.Apellido).ThenBy(s => s.Nombre);
        }

        internal static SocioDTO MapearADto(Socio socio)
        {
            SocioDTO dto = new SocioDTO();
            dto.Id = socio.Id;
            dto.Nombre = socio.Nombre;
            dto.Apellido = socio.Apellido;
            dto.Dni = socio.Dni;
            dto.Telefono = socio.Telefono;
            dto.Email = socio.Email;
            dto.Categoria = socio.Categoria;
            dto.ResponsableId = socio.Responsable?.Id;
            dto.Responsable = socio.Responsable?.ObtenerDescripcion() ?? string.Empty;
            dto.Activo = socio.Activo;
            dto.Descripcion = socio.ObtenerDescripcion();
            return dto;
        }
    }
}
