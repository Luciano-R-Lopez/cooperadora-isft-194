using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Service
{
    // Arma el texto de qué cambió en una modificación, para la auditoría:
    // "Nombre: Ana → Analía; Roles: Operador → Operador, Contador". Solo incluye los campos que cambiaron.
    public class DescripcionCambios
    {
        private readonly List<string> cambios = new List<string>();

        public DescripcionCambios Campo<T>(string campo, T anterior, T nuevo)
        {
            if (!EqualityComparer<T>.Default.Equals(anterior, nuevo))
            {
                cambios.Add(campo + ": " + Mostrar(anterior) + " → " + Mostrar(nuevo));
            }
            return this;
        }

        // Un cambio cuyo valor no se muestra (por ejemplo, la contraseña).
        public DescripcionCambios Oculto(string campo, bool cambio)
        {
            if (cambio)
            {
                cambios.Add(campo + ": modificada");
            }
            return this;
        }

        public bool HayCambios
        {
            get { return cambios.Count > 0; }
        }

        public override string ToString()
        {
            return HayCambios ? string.Join("; ", cambios) : "Sin cambios";
        }

        private static string Mostrar<T>(T valor)
        {
            switch (valor)
            {
                case null:
                    return "(ninguno)";
                case string texto when texto.Length == 0:
                    return "(vacío)";
                case bool siNo:
                    return siNo ? "Sí" : "No";
                case decimal monto:
                    return Formato.Monto(monto);
                case Enum enumerado:
                    return enumerado.Texto();
                default:
                    return valor.ToString() ?? string.Empty;
            }
        }
    }
}
