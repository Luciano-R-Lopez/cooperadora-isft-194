namespace CooperadoraISFT94.Common.Exceptions
{
    /// <summary>Un dato que se buscó por id (o que se esperaba que existiera) no existe.</summary>
    public class NoEncontradoException : NegocioException
    {
        public NoEncontradoException(string mensaje) : base(mensaje)
        {
        }
    }
}
