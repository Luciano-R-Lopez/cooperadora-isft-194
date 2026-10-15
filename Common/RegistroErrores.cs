namespace CooperadoraISFT94.Common
{
    /// <summary>
    /// Guarda los errores inesperados (de programación) en un archivo de texto, para poder saber qué pasó
    /// aunque el usuario cierre el mensaje. Nunca lanza: si no puede escribir, lo ignora.
    /// </summary>
    public static class RegistroErrores
    {
        /// <summary>%LOCALAPPDATA%\CooperadoraISFT94\errores.log</summary>
        public static string Archivo { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "CooperadoraISFT94", "errores.log");

        private static readonly object Candado = new();

        public static void Registrar(Exception excepcion)
        {
            try
            {
                var usuario = SesionActual.Instancia.Usuario?.NombreUsuario ?? "(sin sesión)";
                var texto = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Usuario: {usuario}{Environment.NewLine}" +
                            $"{excepcion}{Environment.NewLine}{new string('-', 80)}{Environment.NewLine}";

                lock (Candado)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Archivo)!);
                    File.AppendAllText(Archivo, texto);
                }
            }
            catch (Exception)
            {
                // El registro es una ayuda: si falla (disco lleno, sin permisos) no debe tapar el error original.
            }
        }
    }
}
