using CooperadoraISFT94.Common;
using MySqlConnector;

namespace CooperadoraISFT94.Repository.MySql
{
    // Conexión con la base y ayudas para ejecutar SQL con parámetros (nunca se arma SQL concatenando datos).
    internal static class BaseDatos
    {
        public static MySqlConnection AbrirConexion()
        {
            MySqlConnection conexion = new MySqlConnection(Configuracion.CadenaConexion);
            conexion.Open();
            return conexion;
        }

        public static void Ejecutar(string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using (MySqlConnection conexion = AbrirConexion())
            using (MySqlCommand comando = Comando(conexion, null, sql, parametros))
            {
                comando.ExecuteNonQuery();
            }
        }

        // Varias sentencias que se guardan todas o ninguna.
        public static void EnTransaccion(Action<MySqlConnection, MySqlTransaction> sentencias)
        {
            using (MySqlConnection conexion = AbrirConexion())
            using (MySqlTransaction transaccion = conexion.BeginTransaction())
            {
                sentencias(conexion, transaccion);
                transaccion.Commit();
            }
        }

        public static MySqlCommand Comando(MySqlConnection conexion, MySqlTransaction? transaccion, string sql,
            params (string Nombre, object? Valor)[] parametros)
        {
            MySqlCommand comando = new MySqlCommand(sql, conexion, transaccion);
            foreach ((string nombre, object? valor) in parametros)
            {
                comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            }
            return comando;
        }

        // Quién hizo el cambio, para las columnas "creado_por", "abierta_por", etc.
        // Null si nadie inició sesión (por ejemplo, al crear el primer usuario).
        public static int? UsuarioDeLaSesion()
        {
            return SesionActual.Instancia.Usuario?.Id;
        }

        // Para las columnas obligatorias: todo cambio desde la aplicación lo hace un usuario con sesión.
        public static int ExigirUsuarioDeLaSesion()
        {
            int? id = UsuarioDeLaSesion();
            if (id == null)
            {
                throw new InvalidOperationException("No hay un usuario con sesión iniciada para registrar el cambio.");
            }
            return id.Value;
        }

        // Los enums se guardan en minúscula: Rol.Administrador -> 'administrador'.
        public static string Nombre(Enum valor)
        {
            return valor.ToString().ToLowerInvariant();
        }

        // Los textos opcionales vacíos se guardan como NULL.
        public static string? TextoONulo(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? null : texto;
        }
    }
}
