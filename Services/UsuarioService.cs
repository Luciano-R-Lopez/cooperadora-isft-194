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
            "No podés modificar tu propio rol ni desactivarte. Pedíselo a otro administrador.";

        private static readonly Lazy<UsuarioService> _instancia = new(() =>
            new UsuarioService(UsuarioRepository.Instancia, AuditoriaService.Instancia));
        public static UsuarioService Instancia => _instancia.Value;

        private readonly IUsuarioRepository _usuarios;
        private readonly IAuditoriaService _auditoria;

        private UsuarioService(IUsuarioRepository usuarios, IAuditoriaService auditoria)
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
                                      PasswordHasher.Hashear(dto.Password), dto.Activo, dto.Rol);

            if (_usuarios.ObtenerPorNombreUsuario(usuario.NombreUsuario) is not null)
                throw new ReglaNegocioException("Ya existe un usuario con ese nombre de usuario.");

            _usuarios.Agregar(usuario);
            _auditoria.Registrar(actorId, AccionAuditoria.Alta, nameof(Usuario), usuario.Id);

            return UsuarioMapper.ADto(usuario);
        }

        public UsuarioDto Editar(UsuarioEditarDto dto, int actorId)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var usuario = _usuarios.ObtenerExistente(dto.Id);

            var cambiaPassword = !string.IsNullOrEmpty(dto.NuevaPassword);
            if (cambiaPassword)
                ValidarPassword(dto.NuevaPassword!);

            // Evita que el único administrador se quite el acceso por error.
            if (dto.Id == actorId && (!dto.Activo || usuario.Rol != dto.Rol))
                throw new ReglaNegocioException(MensajeAutoModificacion);

            // Valida todos los datos antes de tocar al usuario: si alguno es inválido, queda sin cambios.
            usuario.ActualizarDatos(dto.Nombre, dto.Apellido, dto.Activo, dto.Rol);
            if (cambiaPassword)
                usuario.CambiarPasswordHash(PasswordHasher.Hashear(dto.NuevaPassword!));

            _usuarios.Actualizar(usuario);
            _auditoria.Registrar(actorId, AccionAuditoria.Modificacion, nameof(Usuario), usuario.Id);

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
            _auditoria.Registrar(actorId, accion, nameof(Usuario), usuario.Id);

            return UsuarioMapper.ADto(usuario);
        }

        private static void ValidarPassword(string? password)
        {
            if (!Validaciones.EsPasswordValida(password ?? string.Empty))
                throw new ReglaNegocioException("La contraseña debe tener entre 6 y 64 caracteres.");
        }
    }
}
