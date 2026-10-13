namespace CooperadoraISFT94.DTOs.Catalogos
{
    /// <summary>Opción de medio de pago para desplegables y filtros.</summary>
    public record MedioPagoDto(int Id, string Nombre)
    {
        public override string ToString() => Nombre;
    }
}
