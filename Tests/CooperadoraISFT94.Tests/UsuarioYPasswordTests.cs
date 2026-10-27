using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Models;

namespace CooperadoraISFT94.Tests
{
    public class UsuarioYPasswordTests
    {
        [Fact]
        public void Hash_verifica_la_contrasenia_correcta_y_rechaza_otra()
        {
            var hash = PasswordHasher.Hashear("secreta123");

            Assert.True(PasswordHasher.Verificar("secreta123", hash));
            Assert.False(PasswordHasher.Verificar("otra", hash));
        }

        [Theory]
        [InlineData("")]
        [InlineData("sin-puntos")]
        [InlineData("abc.c2FsdA==.aGFzaA==")]
        [InlineData("0.c2FsdA==.aGFzaA==")]
        [InlineData("100.no-es-base64!.aGFzaA==")]
        [InlineData("100..")]
        public void Hash_guardado_con_formato_invalido_no_verifica_ni_lanza(string hashGuardado)
        {
            Assert.False(PasswordHasher.Verificar("cualquiera", hashGuardado));
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("con espacio")]
        public void Nombre_de_usuario_invalido_se_rechaza(string nombreUsuario)
        {
            Assert.Throws<ReglaNegocioException>(() =>
                new Usuario(nombreUsuario, "Ana", "Admin", "hash", true, Rol.Administrador));
        }

        [Fact]
        public void ActualizarDatos_no_modifica_nada_si_un_dato_es_invalido()
        {
            var usuario = Crear.Usuario();

            Assert.Throws<ReglaNegocioException>(() => usuario.ActualizarDatos("Nuevo", "", false, Rol.Contador));

            Assert.Equal("Omar", usuario.Nombre);
            Assert.True(usuario.Activo);
            Assert.Equal(new[] { Rol.Operador }, usuario.Roles);
        }

        [Fact]
        public void Un_usuario_puede_tener_varios_roles_sin_repetirlos()
        {
            var usuario = new Usuario("pablo.paz", "Pablo", "Paz", "hash", true, Rol.Contador, Rol.Operador, Rol.Contador);

            Assert.Equal(new[] { Rol.Operador, Rol.Contador }, usuario.Roles);
            Assert.True(usuario.TieneRol(Rol.Operador));
            Assert.True(usuario.TieneRol(Rol.Contador));
            Assert.False(usuario.TieneRol(Rol.Administrador));
        }

        [Fact]
        public void Un_usuario_sin_roles_se_rechaza_y_al_editar_queda_como_estaba()
        {
            Assert.Throws<ReglaNegocioException>(() => new Usuario("ana", "Ana", "Paz", "hash", true, Array.Empty<Rol>()));

            var usuario = Crear.Usuario();
            Assert.Throws<ReglaNegocioException>(() => usuario.ActualizarDatos("Omar", "Operador", true, Array.Empty<Rol>()));
            Assert.Equal(new[] { Rol.Operador }, usuario.Roles);
        }
    }
}
