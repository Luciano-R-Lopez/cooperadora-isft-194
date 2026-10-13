namespace CooperadoraISFT94.Views.Common
{
    /// <summary>Elemento de desplegable con un texto para mostrar y un valor asociado (por ejemplo, "Todos" = null).</summary>
    public record Opcion<T>(string Texto, T Valor)
    {
        public override string ToString() => Texto;
    }
}
