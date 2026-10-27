namespace CooperadoraISFT94.Dto
{
    /// <summary>Opción de motivo de egreso para desplegables.</summary>
    public record MotivoEgresoDto(int Id, string Nombre)
    {
        public override string ToString() => Nombre;
    }
}
