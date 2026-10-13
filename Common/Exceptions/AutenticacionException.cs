namespace CooperadoraISFT94.Common.Exceptions
{
    /// <summary>No se pudo iniciar sesión (credenciales incorrectas, usuario desactivado, etc.).</summary>
    public class AutenticacionException : NegocioException
    {
        public AutenticacionException(string mensaje) : base(mensaje)
        {
        }
    }
}
