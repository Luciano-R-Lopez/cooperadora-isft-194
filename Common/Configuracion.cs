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

        public static readonly string RutaRegistroErrores = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CooperadoraISFT194", "errores.log");
    }
}
