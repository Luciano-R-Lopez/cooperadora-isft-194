namespace CooperadoraISFT94.Common.Exceptions
{
    // No se pudo iniciar sesión: usuario o contraseña incorrectos, usuario desactivado o bloqueado.
    public class AutenticacionException : NegocioException
    {
        public AutenticacionException(string mensaje) : base(mensaje)
        {
        }
    }
}
