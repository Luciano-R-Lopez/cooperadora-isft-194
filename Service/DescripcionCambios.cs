namespace CooperadoraISFT94.Service
{
    /// <summary>
    /// Arma el texto de qué cambió en una modificación, para la auditoría:
    /// "Nombre: Ana → Analía; Rol: Operador → Contador". Solo incluye los campos que cambiaron.
    /// </summary>
    public sealed class DescripcionCambios
    {
        private readonly List<string> _cambios = new();

        public DescripcionCambios Campo<T>(string campo, T anterior, T nuevo)
        {
            if (!EqualityComparer<T>.Default.Equals(anterior, nuevo))
                _cambios.Add($"{campo}: {Mostrar(anterior)} → {Mostrar(nuevo)}");
            return this;
        }

        /// <summary>Un cambio cuyo valor no se debe mostrar (por ejemplo, la contraseña).</summary>
        public DescripcionCambios Oculto(string campo, bool cambio)
        {
            if (cambio)
                _cambios.Add($"{campo}: modificada");
            return this;
        }

        public bool HayCambios => _cambios.Count > 0;

        public override string ToString() => HayCambios ? string.Join("; ", _cambios) : "Sin cambios";

        private static string Mostrar<T>(T valor) =>
            valor switch
            {
                null => "(ninguno)",
                string s when s.Length == 0 => "(vacío)",
                bool b => b ? "Sí" : "No",
                decimal d => d.ToString("C2"),
                _ => valor.ToString() ?? string.Empty
            };
    }
}
