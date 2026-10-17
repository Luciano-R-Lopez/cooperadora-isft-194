using CooperadoraISFT94.Common;
using MySqlConnector;

namespace CooperadoraISFT94.Repositories.MySql
{
    /// <summary>Atajos para ejecutar comandos con parámetros.</summary>
    internal static class Sql
    {
        /// <summary>Ejecuta un INSERT y devuelve el id autogenerado por la base.</summary>
        public static int Insertar(string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using var conexion = BaseDatos.Abrir();
            using var comando = Crear(conexion, sql, parametros);
            comando.ExecuteNonQuery();
            return (int)comando.LastInsertedId;
        }

        public static void Ejecutar(string sql, params (string Nombre, object? Valor)[] parametros)
        {
            using var conexion = BaseDatos.Abrir();
            using var comando = Crear(conexion, sql, parametros);
            comando.ExecuteNonQuery();
        }

        /// <summary>Id del usuario logueado, o null si no hay sesión.</summary>
        public static object? UsuarioActual() => SesionActual.Instancia.Usuario?.Id;

        private static MySqlCommand Crear(MySqlConnection conexion, string sql, (string Nombre, object? Valor)[] parametros)
        {
            var comando = new MySqlCommand(sql, conexion);
            foreach (var (nombre, valor) in parametros)
                comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            return comando;
        }
    }

    /// <summary>Lectura de columnas por nombre.</summary>
    internal static class Lector
    {
        public static string Texto(this MySqlDataReader r, string columna) => r.GetString(r.GetOrdinal(columna));

        public static string? TextoONulo(this MySqlDataReader r, string columna)
        {
            var i = r.GetOrdinal(columna);
            return r.IsDBNull(i) ? null : r.GetString(i);
        }

        public static int Entero(this MySqlDataReader r, string columna) => r.GetInt32(r.GetOrdinal(columna));

        public static int? EnteroONulo(this MySqlDataReader r, string columna)
        {
            var i = r.GetOrdinal(columna);
            return r.IsDBNull(i) ? null : r.GetInt32(i);
        }

        public static decimal Numero(this MySqlDataReader r, string columna) => r.GetDecimal(r.GetOrdinal(columna));

        public static decimal? NumeroONulo(this MySqlDataReader r, string columna)
        {
            var i = r.GetOrdinal(columna);
            return r.IsDBNull(i) ? null : r.GetDecimal(i);
        }

        public static DateTime Fecha(this MySqlDataReader r, string columna) => r.GetDateTime(r.GetOrdinal(columna));

        public static DateTime? FechaONula(this MySqlDataReader r, string columna)
        {
            var i = r.GetOrdinal(columna);
            return r.IsDBNull(i) ? null : r.GetDateTime(i);
        }

        public static bool Si(this MySqlDataReader r, string columna) => r.GetBoolean(r.GetOrdinal(columna));
    }
}
