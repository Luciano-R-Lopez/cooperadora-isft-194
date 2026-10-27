namespace CooperadoraISFT94.Common.Exceptions
{
    // El usuario que inició sesión no tiene el rol que pide la acción.
    public class AccesoDenegadoException : NegocioException
    {
        public AccesoDenegadoException() : base("No tiene permisos para realizar esta acción.")
        {
        }
    }
}
