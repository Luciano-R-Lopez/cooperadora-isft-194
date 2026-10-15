using System.Text;

namespace CooperadoraISFT94.Views.Common
{
    /// <summary>
    /// Exporta lo que muestra una grilla a un archivo CSV que abre Excel: mismas columnas y mismo formato
    /// que en pantalla. Usa ";" como separador (el que espera Excel en español) y UTF-8 con BOM para los acentos.
    /// </summary>
    public static class ExportarCsv
    {
        private const char Separador = ';';

        /// <summary>Pide dónde guardar y escribe el archivo. Devuelve false si el usuario canceló.</summary>
        public static bool Grilla(DataGridView grilla, string nombreSugerido)
        {
            using var dialogo = new SaveFileDialog
            {
                Title = "Exportar a CSV",
                Filter = "CSV (para Excel) (*.csv)|*.csv",
                FileName = $"{nombreSugerido} {DateTime.Now:yyyy-MM-dd}.csv",
                OverwritePrompt = true
            };

            if (dialogo.ShowDialog() != DialogResult.OK)
                return false;

            File.WriteAllText(dialogo.FileName, ArmarTexto(grilla), new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
            return true;
        }

        /// <summary>El contenido del CSV, separado de la escritura para poder probarlo.</summary>
        public static string ArmarTexto(DataGridView grilla)
        {
            var columnas = grilla.Columns.Cast<DataGridViewColumn>()
                                 .Where(c => c.Visible)
                                 .OrderBy(c => c.DisplayIndex)
                                 .ToList();

            var texto = new StringBuilder();
            texto.AppendLine(string.Join(Separador, columnas.Select(c => Escapar(c.HeaderText))));

            foreach (DataGridViewRow fila in grilla.Rows)
            {
                if (fila.IsNewRow) continue;
                texto.AppendLine(string.Join(Separador,
                    columnas.Select(c => Escapar(fila.Cells[c.Index].FormattedValue?.ToString()))));
            }

            return texto.ToString();
        }

        /// <summary>Entre comillas si el valor tiene separador, comillas o saltos de línea (regla de CSV).</summary>
        private static string Escapar(string? valor)
        {
            var texto = valor ?? string.Empty;
            return texto.IndexOfAny(new[] { Separador, '"', '\n', '\r' }) >= 0
                ? $"\"{texto.Replace("\"", "\"\"")}\""
                : texto;
        }
    }
}
