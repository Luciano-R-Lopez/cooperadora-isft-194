using MySqlConnector;

namespace CooperadoraISFT94.Repositories.MySql
{
    public static class BaseDatos
    {
        private const string ConexionPorDefecto =
            "Server=localhost;Port=3306; Database=cooperadora; User ID=root;Password=;";

        /// <summary>
        /// Por defecto usa el MySQL de XAMPP (usuario root sin contraseña).
        /// Si alguien del grupo tiene otra configuración, puede definir la variable de entorno
        /// COOPERADORA_CONEXION con su propio connection string sin tocar el código.
        /// </summary>
        public static string CadenaConexion =>
            Environment.GetEnvironmentVariable("COOPERADORA_CONEXION") ?? ConexionPorDefecto;

        public static MySqlConnection Abrir()
        {
            var conexion = new MySqlConnection(CadenaConexion);
            conexion.Open();
            return conexion;
        }

        /// <summary>Lee la base, llena los repositorios y a partir de ahí guarda cada cambio en MySQL.</summary>
        public static void Inicializar() => CargadorDeDatos.Cargar();
    }
}