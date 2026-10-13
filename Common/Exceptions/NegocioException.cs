namespace CooperadoraISFT94.Common.Exceptions
{
    /// <summary>
    /// Base de los errores que se le muestran al usuario (reglas incumplidas, datos que no existen, permisos).
    /// Cualquier otra excepción es un error de programación y no debe atraparse en las vistas.
    /// </summary>
    public class NegocioException : Exception
    {
        public NegocioException(string mensaje) : base(mensaje)
        {
        }
    }
}
