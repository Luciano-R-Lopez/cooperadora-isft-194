using System.ComponentModel;
using System.Reflection;

namespace CooperadoraISFT94.Common
{
    // Texto para mostrar de un valor de enumeración: el de su [Description], o el nombre si no tiene.
    public static class TextoEnum
    {
        public static string Texto(this Enum valor)
        {
            FieldInfo? campo = valor.GetType().GetField(valor.ToString());
            return campo?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? valor.ToString();
        }

        public static string? Texto(object? valor)
        {
            return valor is Enum enumerado ? enumerado.Texto() : valor?.ToString();
        }
    }
}
