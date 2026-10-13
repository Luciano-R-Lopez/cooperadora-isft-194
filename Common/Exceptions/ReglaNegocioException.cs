namespace CooperadoraISFT94.Common.Exceptions
{
    /// <summary>Se incumple una regla del negocio (dato inválido, duplicado, operación no permitida).</summary>
    public class ReglaNegocioException : NegocioException
    {
        public ReglaNegocioException(string mensaje) : base(mensaje)
        {
        }
    }
}
