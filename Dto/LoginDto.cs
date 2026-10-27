namespace CooperadoraISFT94.Dto
{
    /// <summary>Datos que viajan de la vista al servicio al iniciar sesión.</summary>
    public record LoginDto(string NombreUsuario, string Password);
}
