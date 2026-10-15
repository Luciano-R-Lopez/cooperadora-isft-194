using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.DTOs.Catalogos
{
    /// <summary>Ítem de un catálogo para la pantalla de administración.</summary>
    public record ItemCatalogoDto(int Id, TipoCatalogo Tipo, string Nombre, bool Activo)
    {
        public string EstadoTexto => Activo ? "Activo" : "Inactivo";
    }
}
