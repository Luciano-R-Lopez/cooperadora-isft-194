namespace CooperadoraISFT94.Common.Exceptions
{
    /// <summary>El usuario logueado no tiene el rol necesario para la acción pedida.</summary>
    public class AccesoDenegadoException : NegocioException
    {
        public AccesoDenegadoException()
            : base("No tenés permisos para realizar esta acción.")
        {
        }
    }
}
