namespace CooperadoraISFT94.Common
{
    public static class Configuracion
    {
        public const string NombreInstitucion = "Cooperadora ISFT 194";

        // Intentos fallidos seguidos que bloquean el ingreso con un nombre de usuario, y cuánto dura el bloqueo.
        public const int IntentosLoginPermitidos = 5;
        public const int MinutosBloqueoLogin = 5;

        // Años que se aceptan para las cuotas.
        public const int AnioMinimo = 2000;
        public const int AnioMaximo = 2100;

        // true: los datos se leen y se guardan en la base MySQL (BaseDeDatos/cooperadora.sql).
        // false: todo queda en memoria, con datos de ejemplo en modo Debug (sirve para probar sin XAMPP).
        public static readonly bool UsarBaseDeDatos = true;

        // Base `cooperadora` del MySQL de XAMPP, con el usuario root sin contraseña que trae por defecto.
        // Quien tenga otra contraseña o puerto puede definir la variable de entorno COOPERADORA_CONEXION.
        public static readonly string CadenaConexion =
            Environment.GetEnvironmentVariable("COOPERADORA_CONEXION")
            ?? "Server=localhost;Port=3306;Database=cooperadora;User ID=root;Password=;Connection Timeout=5";

        public static readonly string RutaRegistroErrores = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CooperadoraISFT194", "errores.log");
    }
}
