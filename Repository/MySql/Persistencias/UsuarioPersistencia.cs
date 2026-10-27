using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Domain;
using MySqlConnector;

namespace CooperadoraISFT94.Repository.MySql.Persistencias
{
    // Los roles van en usuario_rol: se borran y se vuelven a escribir junto con el usuario.
    internal class UsuarioPersistencia : IPersistencia<Usuario>
    {
        public void Insertar(Usuario usuario)
        {
            BaseDatos.EnTransaccion((conexion, transaccion) =>
            {
                using (MySqlCommand comando = BaseDatos.Comando(conexion, transaccion,
                    @"INSERT INTO usuario (id_usuario, nombre_usuario, nombre, apellido, password_hash, activo, creado_en, creado_por)
                      VALUES (@id, @usuario, @nombre, @apellido, @hash, @activo, NOW(), @por)",
                    ("@id", usuario.Id), ("@usuario", usuario.NombreUsuario), ("@nombre", usuario.Nombre),
                    ("@apellido", usuario.Apellido), ("@hash", usuario.PasswordHash), ("@activo", usuario.Activo),
                    ("@por", BaseDatos.UsuarioDeLaSesion())))
                {
                    comando.ExecuteNonQuery();
                }
                GuardarRoles(conexion, transaccion, usuario);
            });
        }

        public void Actualizar(Usuario usuario)
        {
            BaseDatos.EnTransaccion((conexion, transaccion) =>
            {
                using (MySqlCommand comando = BaseDatos.Comando(conexion, transaccion,
                    @"UPDATE usuario
                      SET nombre = @nombre, apellido = @apellido, password_hash = @hash, activo = @activo, actualizado_en = NOW()
                      WHERE id_usuario = @id",
                    ("@nombre", usuario.Nombre), ("@apellido", usuario.Apellido), ("@hash", usuario.PasswordHash),
                    ("@activo", usuario.Activo), ("@id", usuario.Id)))
                {
                    comando.ExecuteNonQuery();
                }
                GuardarRoles(conexion, transaccion, usuario);
            });
        }

        public void Eliminar(int id)
        {
            throw new NotSupportedException("Los usuarios no se eliminan: se desactivan.");
        }

        private static void GuardarRoles(MySqlConnection conexion, MySqlTransaction transaccion, Usuario usuario)
        {
            using (MySqlCommand borrar = BaseDatos.Comando(conexion, transaccion,
                "DELETE FROM usuario_rol WHERE id_usuario = @id", ("@id", usuario.Id)))
            {
                borrar.ExecuteNonQuery();
            }
            foreach (Rol rol in usuario.Roles)
            {
                using (MySqlCommand agregar = BaseDatos.Comando(conexion, transaccion,
                    "INSERT INTO usuario_rol (id_usuario, id_rol) SELECT @id, id_rol FROM rol WHERE nombre = @rol",
                    ("@id", usuario.Id), ("@rol", BaseDatos.Nombre(rol))))
                {
                    if (agregar.ExecuteNonQuery() == 0)
                    {
                        throw new InvalidOperationException("La tabla rol no tiene el rol '" + BaseDatos.Nombre(rol) + "'.");
                    }
                }
            }
        }
    }
}
