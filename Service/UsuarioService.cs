using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Domain;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Repository;

namespace CooperadoraISFT94.Service
{
    public class UsuarioService
    {
        private const string MensajeAutoModificacion =
            "No puede quitarse el rol de Administrador ni desactivarse. Pídaselo a otro administrador.";

        private readonly IRepository<Usuario> repositorio;
        private readonly AuditoriaService auditoria;

        public UsuarioService()
            : this(Repositorios.Usuarios, new AuditoriaService())
        {
        }

        public UsuarioService(IRepository<Usuario> repositorio, AuditoriaService auditoria)
        {
            this.repositorio = repositorio;
            this.auditoria = auditoria;
        }

        public List<UsuarioDTO> ObtenerTodos()
        {
            return repositorio.ObtenerTodos()
                              .OrderBy(u => u.Apellido)
                              .ThenBy(u => u.Nombre)
                              .Select(MapearADto)
                              .ToList();
        }

        public UsuarioDTO ObtenerPorId(int id)
        {
            return MapearADto(repositorio.ObtenerExistente(id));
        }

        // Crea el usuario si dto.Id es 0; si no, lo edita. actorId: el administrador que hace el cambio.
        public UsuarioDTO Guardar(UsuarioDTO dto, int actorId)
        {
            if (dto == null)
            {
                throw new ArgumentException("Los datos del usuario son obligatorios.");
            }
            return dto.Id == 0 ? Crear(dto, actorId) : Editar(dto, actorId);
        }

        private UsuarioDTO Crear(UsuarioDTO dto, int actorId)
        {
            ValidarPassword(dto.Password);
            Usuario nuevo = new Usuario(repositorio.ObtenerProximoId(), dto.NombreUsuario, dto.Nombre, dto.Apellido,
                PasswordHasher.Hashear(dto.Password), dto.Activo, dto.Roles);
            if (ExisteNombreUsuario(nuevo.NombreUsuario))
            {
                throw new NegocioException("Ya existe un usuario con ese nombre de usuario.");
            }

            repositorio.Agregar(nuevo);
            auditoria.Registrar(actorId, AccionAuditoria.Alta, nameof(Usuario), nuevo.Id,
                nuevo.NombreUsuario + " (" + string.Join(", ", nuevo.Roles) + ")");
            return MapearADto(nuevo);
        }

        private UsuarioDTO Editar(UsuarioDTO dto, int actorId)
        {
            Usuario usuario = repositorio.ObtenerExistente(dto.Id);
            bool cambiaPassword = !string.IsNullOrEmpty(dto.Password);
            if (cambiaPassword)
            {
                ValidarPassword(dto.Password);
            }

            // Para que el único administrador no se quite el acceso por error. Sí puede sumarse o quitarse otros roles.
            if (dto.Id == actorId && (!dto.Activo || !dto.Roles.Contains(Rol.Administrador)))
            {
                throw new NegocioException(MensajeAutoModificacion);
            }

            // Se validan todos los datos con un usuario aparte antes de tocar el guardado.
            Usuario validado = new Usuario(usuario.Id, usuario.NombreUsuario, dto.Nombre, dto.Apellido,
                usuario.PasswordHash, dto.Activo, dto.Roles);
            UsuarioDTO anterior = MapearADto(usuario);

            usuario.Nombre = validado.Nombre;
            usuario.Apellido = validado.Apellido;
            usuario.Activo = validado.Activo;
            usuario.AsignarRoles(validado.Roles);
            if (cambiaPassword)
            {
                usuario.PasswordHash = PasswordHasher.Hashear(dto.Password);
            }
            repositorio.Actualizar(usuario);

            UsuarioDTO nuevo = MapearADto(usuario);
            DescripcionCambios cambios = new DescripcionCambios()
                .Campo("Nombre", anterior.Nombre, nuevo.Nombre)
                .Campo("Apellido", anterior.Apellido, nuevo.Apellido)
                .Campo("Activo", anterior.Activo, nuevo.Activo)
                .Campo("Roles", anterior.RolesTexto, nuevo.RolesTexto)
                .Oculto("Contraseña", cambiaPassword);
            auditoria.Registrar(actorId, AccionAuditoria.Modificacion, nameof(Usuario), usuario.Id, cambios.ToString());
            return nuevo;
        }

        public UsuarioDTO CambiarEstado(int usuarioId, bool activo, int actorId)
        {
            Usuario usuario = repositorio.ObtenerExistente(usuarioId);
            if (usuarioId == actorId && !activo)
            {
                throw new NegocioException(MensajeAutoModificacion);
            }

            usuario.Activo = activo;
            repositorio.Actualizar(usuario);

            // Desactivar es la baja lógica del usuario; reactivarlo es una modificación.
            auditoria.Registrar(actorId, activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja, nameof(Usuario),
                usuario.Id, activo ? "Activado" : "Desactivado");
            return MapearADto(usuario);
        }

        public void CambiarPasswordPropia(int usuarioId, CambioPasswordDTO dto)
        {
            Usuario usuario = repositorio.ObtenerExistente(usuarioId);
            if (!PasswordHasher.Verificar(dto.Actual ?? string.Empty, usuario.PasswordHash))
            {
                throw new NegocioException("La contraseña actual no es correcta.");
            }
            ValidarPassword(dto.Nueva);
            if (dto.Nueva != dto.Confirmacion)
            {
                throw new NegocioException("La nueva contraseña y su confirmación no coinciden.");
            }
            if (dto.Nueva == dto.Actual)
            {
                throw new NegocioException("La nueva contraseña tiene que ser distinta de la actual.");
            }

            usuario.PasswordHash = PasswordHasher.Hashear(dto.Nueva);
            repositorio.Actualizar(usuario);
            auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Usuario), usuario.Id,
                "Cambió su propia contraseña");
        }

        // La contraseña nunca sale del servicio: el DTO va sin ella.
        internal static UsuarioDTO MapearADto(Usuario usuario)
        {
            UsuarioDTO dto = new UsuarioDTO();
            dto.Id = usuario.Id;
            dto.NombreUsuario = usuario.NombreUsuario;
            dto.Nombre = usuario.Nombre;
            dto.Apellido = usuario.Apellido;
            dto.Activo = usuario.Activo;
            dto.Roles = usuario.Roles.ToList();
            return dto;
        }

        private bool ExisteNombreUsuario(string nombreUsuario)
        {
            return repositorio.ObtenerTodos().Any(u =>
                string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase));
        }

        private static void ValidarPassword(string? password)
        {
            if (!Validaciones.EsPasswordValida(password ?? string.Empty))
            {
                throw new NegocioException("La contraseña debe tener entre 6 y 64 caracteres.");
            }
        }
    }
}
