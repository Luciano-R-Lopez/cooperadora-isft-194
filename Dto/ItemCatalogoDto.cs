using CooperadoraISFT94.Common.Enums;

namespace CooperadoraISFT94.Dto
{
    /// <summary>Ítem de un catálogo para la pantalla de administración.</summary>
    public record ItemCatalogoDto(int Id, TipoCatalogo Tipo, string Nombre, bool Activo)
    {
        public string EstadoTexto => Activo ? "Activo" : "Inactivo";
    }
}
