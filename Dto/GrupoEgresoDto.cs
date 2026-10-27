namespace CooperadoraISFT94.Dto
{
    /// <summary>Opción de grupo de egreso para desplegables.</summary>
    public record GrupoEgresoDto(int Id, string Nombre)
    {
        public override string ToString() => Nombre;
    }
}
