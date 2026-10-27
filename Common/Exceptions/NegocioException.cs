namespace CooperadoraISFT94.Common.Exceptions
{
    // Error que se le muestra al usuario: una regla del negocio que no se cumple (dato inválido, repetido, etc.).
    // Cualquier otra excepción es un error de programación y la atrapa Program.
    public class NegocioException : Exception
    {
        public NegocioException(string mensaje) : base(mensaje)
        {
        }
    }
}
