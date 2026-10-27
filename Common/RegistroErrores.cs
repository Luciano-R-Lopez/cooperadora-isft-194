namespace CooperadoraISFT94.Common
{
    // Guarda los errores inesperados en un archivo de texto para poder saber qué pasó aunque el usuario
    // cierre el mensaje. Nunca lanza: si no puede escribir, lo ignora.
    public static class RegistroErrores
    {
        private static readonly object candado = new object();

        public static void Registrar(Exception excepcion)
        {
            try
            {
                string usuario = SesionActual.Instancia.Usuario?.NombreUsuario ?? "(sin sesión)";
                string texto = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] Usuario: " + usuario + Environment.NewLine
                    + excepcion + Environment.NewLine + new string('-', 80) + Environment.NewLine;

                lock (candado)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Configuracion.RutaRegistroErrores)!);
                    File.AppendAllText(Configuracion.RutaRegistroErrores, texto);
                }
            }
            catch (Exception)
            {
                // El registro es una ayuda: si falla (disco lleno, sin permisos) no debe tapar el error original.
            }
        }
    }
}
