using System.Globalization;
using System.Text;

namespace CooperadoraISFT94.Forms.Utils
{
    internal static class Busqueda
    {
        // Coincide si cada palabra buscada aparece en alguno de los campos, sin distinguir mayúsculas ni tildes.
        public static bool Coincide(string texto, params string?[] campos)
        {
            string[] palabras = Normalizar(texto).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string palabra in palabras)
            {
                bool encontrada = false;
                foreach (string? campo in campos)
                {
                    if (Normalizar(campo).Contains(palabra))
                    {
                        encontrada = true;
                        break;
                    }
                }
                if (!encontrada)
                {
                    return false;
                }
            }
            return true;
        }

        private static string Normalizar(string? texto)
        {
            string descompuesto = (texto ?? string.Empty).Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            StringBuilder resultado = new StringBuilder(descompuesto.Length);
            foreach (char caracter in descompuesto)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(caracter) != UnicodeCategory.NonSpacingMark)
                {
                    resultado.Append(caracter);
                }
            }
            return resultado.ToString();
        }
    }
}
