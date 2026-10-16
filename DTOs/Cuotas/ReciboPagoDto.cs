namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>Una cuota pagada dentro de un recibo.</summary>
    public record LineaReciboDto(int CuotaId, string Socio, int Anio, decimal Monto);

    /// <summary>
    /// Datos del recibo de un pago: una o varias cuotas cobradas juntas en un solo ingreso.
    /// El número es el del ingreso registrado en la caja.
    /// </summary>
    public record ReciboPagoDto(
        int Numero,
        DateTime Fecha,
        IReadOnlyList<LineaReciboDto> Cuotas,
        string MedioPago,
        string? PagadoPor,
        string Usuario)
    {
        /// <summary>Total cobrado.</summary>
        public decimal Monto => Cuotas.Sum(c => c.Monto);

        /// <summary>El socio (o los socios, si el responsable pagó por varios).</summary>
        public string Socio => string.Join("; ", Cuotas.Select(c => c.Socio).Distinct());
    }
}
