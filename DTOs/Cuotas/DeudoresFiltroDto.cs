namespace CooperadoraISFT94.DTOs.Cuotas
{
    /// <summary>Filtros de la lista de morosos. Anio null = todos los años; Texto busca por nombre o DNI.</summary>
    public record DeudoresFiltroDto(int? Anio, string? Texto, bool IncluirDadosDeBaja);
}
