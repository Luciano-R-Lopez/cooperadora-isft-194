namespace CooperadoraISFT94.Dto
{
    // Anio null = todos los años. Texto busca por nombre, DNI o responsable.
    public class DeudoresFiltroDTO
    {
        public int? Anio { get; set; }

        public string Texto { get; set; } = string.Empty;

        public bool IncluirDadosDeBaja { get; set; }
    }
}
