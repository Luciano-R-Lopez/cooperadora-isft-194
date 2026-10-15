namespace CooperadoraISFT94.DTOs.Usuarios
{
    /// <summary>Cambio de la propia contraseña: la actual (para confirmar identidad) y la nueva dos veces.</summary>
    public record CambioPasswordDto(string Actual, string Nueva, string Confirmacion);
}
