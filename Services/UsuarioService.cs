using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Repositories.Interfaces;
using CooperadoraISFT94.Services.Interfaces;

namespace CooperadoraISFT94.Services
{
    public sealed class UsuarioService : IUsuarioService
    {
        private const string MensajeAutoModificacion =
            "No podés quitarte el rol de Administrador ni desactivarte. Pedíselo a otro administrador.";

        private static readonly Lazy<UsuarioService> _instancia = new(() =>
            new UsuarioService(UsuarioRepository.Instancia, AuditoriaService.Instancia));
        public static UsuarioService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly IAuditoriaService _auditoria;

        internal UsuarioService(IUsuarioRepository usuarios, IAuditoriaService auditoria)
        {
            _usuarios = usuarios;
            _auditoria = auditoria;
        }

        public IReadOnlyList<UsuarioDto> ObtenerTodos() =>
            _usuarios.ObtenerTodos()
                     .OrderBy(u => u.Apellido)
                     .ThenBy(u => u.Nombre)
                     .Select(UsuarioMapper.ADto)
                     .ToList();

        public UsuarioDto Registrar(UsuarioCrearDto dto, int actorId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            // La contraseña en texto plano solo la ve el servicio: se valida y se guarda su hash.
            // Los demás datos los valida el propio Usuario.
            ValidarPassword(dto.Password);
            var usuario = new Usuario(dto.NombreUsuario, dto.Nombre, dto.Apellido,
                                      PasswordHasher.Hashear(dto.Password), dto.Activo, dto.Roles ?? Array.Empty<Rol>());

            if (_usuarios.ObtenerPorNombreUsuario(usuario.NombreUsuario) is not null)
                throw new ReglaNegocioException("Ya existe un usuario con ese nombre de usuario.");

            _usuarios.Agregar(usuario);
            _auditoria.Registrar(actorId, AccionAuditoria.Alta, nameof(Usuario), usuario.Id,
                                 $"{usuario.NombreUsuario} ({string.Join(", ", usuario.Roles)})");

            return UsuarioMapper.ADto(usuario);
        }

        public UsuarioDto Editar(UsuarioEditarDto dto, int actorId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var usuario = _usuarios.ObtenerExistente(dto.Id);

            var cambiaPassword = !string.IsNullOrEmpty(dto.NuevaPassword);
            if (cambiaPassword)
                ValidarPassword(dto.NuevaPassword!);

            // Evita que el único administrador se quite el acceso por error. Sí puede sumarse o quitarse otros roles.
            var roles = dto.Roles ?? Array.Empty<Rol>();
            if (dto.Id == actorId && (!dto.Activo || !roles.Contains(Rol.Administrador)))
                throw new ReglaNegocioException(MensajeAutoModificacion);

            var anterior = UsuarioMapper.ADto(usuario);

            // Valida todos los datos antes de tocar al usuario: si alguno es inválido, queda sin cambios.
            usuario.ActualizarDatos(dto.Nombre, dto.Apellido, dto.Activo, roles);
            if (cambiaPassword)
                usuario.CambiarPasswordHash(PasswordHasher.Hashear(dto.NuevaPassword!));

            _usuarios.Actualizar(usuario);

            var nuevo = UsuarioMapper.ADto(usuario);
            var cambios = new DescripcionCambios()
                .Campo("Nombre", anterior.Nombre, nuevo.Nombre)
                .Campo("Apellido", anterior.Apellido, nuevo.Apellido)
                .Campo("Activo", anterior.Activo, nuevo.Activo)
                .Campo("Roles", anterior.RolesTexto, nuevo.RolesTexto)
                .Oculto("Contraseña", cambiaPassword);
            _auditoria.Registrar(actorId, AccionAuditoria.Modificacion, nameof(Usuario), usuario.Id, cambios.ToString());

            return UsuarioMapper.ADto(usuario);
        }

        public UsuarioDto CambiarEstado(int usuarioId, bool activo, int actorId)
        {
            var usuario = _usuarios.ObtenerExistente(usuarioId);

            if (usuarioId == actorId && !activo)
                throw new ReglaNegocioException(MensajeAutoModificacion);

            if (activo)
                usuario.Activar();
            else
                usuario.Desactivar();

            _usuarios.Actualizar(usuario);

            // Desactivar es la baja lógica del usuario; reactivarlo es una modificación.
            var accion = activo ? AccionAuditoria.Modificacion : AccionAuditoria.Baja;
            _auditoria.Registrar(actorId, accion, nameof(Usuario), usuario.Id, activo ? "Activado" : "Desactivado");

            return UsuarioMapper.ADto(usuario);
        }

        public void CambiarPasswordPropia(int usuarioId, CambioPasswordDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var usuario = _usuarios.ObtenerExistente(usuarioId);

            if (!PasswordHasher.Verificar(dto.Actual ?? string.Empty, usuario.PasswordHash))
                throw new ReglaNegocioException("La contraseña actual no es correcta.");
            ValidarPassword(dto.Nueva);
            if (dto.Nueva != dto.Confirmacion)
                throw new ReglaNegocioException("La nueva contraseña y su confirmación no coinciden.");
            if (dto.Nueva == dto.Actual)
                throw new ReglaNegocioException("La nueva contraseña tiene que ser distinta de la actual.");

            usuario.CambiarPasswordHash(PasswordHasher.Hashear(dto.Nueva));
            _usuarios.Actualizar(usuario);
            _auditoria.Registrar(usuarioId, AccionAuditoria.Modificacion, nameof(Usuario), usuario.Id,
                                 "Cambió su propia contraseña");
        }

        private static void ValidarPassword(string? password)
        {
            if (!Validaciones.EsPasswordValida(password ?? string.Empty))
                throw new ReglaNegocioException("La contraseña debe tener entre 6 y 64 caracteres.");
        }
    }
}
