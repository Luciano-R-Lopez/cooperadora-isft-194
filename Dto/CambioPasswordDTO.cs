namespace CooperadoraISFT94.Dto
{
    // La contraseña actual confirma la identidad; la nueva se escribe dos veces.
    public class CambioPasswordDTO
    {
        public string Actual { get; set; } = string.Empty;

        public string Nueva { get; set; } = string.Empty;

        public string Confirmacion { get; set; } = string.Empty;
    }
}
