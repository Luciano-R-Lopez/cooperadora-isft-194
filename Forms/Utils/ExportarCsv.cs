using System.Text;
using CooperadoraISFT94.Dto;

namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>
    /// Exporta datos a un archivo CSV que abre Excel. Usa ";" como separador (el que espera Excel en español)
    /// y UTF-8 con BOM para los acentos.
    /// </summary>
    public static class ExportarCsv
    {
        private const char Separador = ';';

        /// <summary>Lo que muestra una grilla: mismas columnas y mismo formato que en pantalla. False si se canceló.</summary>
        public static bool Grilla(DataGridView grilla, string nombreSugerido) =>
            Guardar(ArmarTexto(grilla), nombreSugerido);

        /// <summary>
        /// Egresos de un período organizados por grupo: primero un resumen con el total de cada grupo
        /// y después el detalle de cada uno con su subtotal. False si se canceló.
        /// </summary>
        public static bool EgresosPorGrupo(IReadOnlyList<MovimientoDto> egresos, DateTime desde, DateTime hasta) =>
            Guardar(ArmarEgresosPorGrupo(egresos, desde, hasta),
                    $"Egresos por grupo {desde:yyyy-MM-dd} a {hasta:yyyy-MM-dd}", conFecha: false);

        /// <summary>Pide dónde guardar y escribe el archivo. Devuelve false si el usuario canceló.</summary>
        private static bool Guardar(string contenido, string nombreSugerido, bool conFecha = true)
        {
            using var dialogo = new SaveFileDialog
            {
                Title = "Exportar a CSV",
                Filter = "CSV (para Excel) (*.csv)|*.csv",
                FileName = conFecha ? $"{nombreSugerido} {DateTime.Now:yyyy-MM-dd}.csv" : $"{nombreSugerido}.csv",
                OverwritePrompt = true
            };

            if (dialogo.ShowDialog() != DialogResult.OK)
                return false;

            File.WriteAllText(dialogo.FileName, contenido, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true));
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
            Fila(texto, columnas.Select(c => c.HeaderText));

            foreach (DataGridViewRow fila in grilla.Rows)
            {
                if (fila.IsNewRow) continue;
                Fila(texto, columnas.Select(c => fila.Cells[c.Index].FormattedValue?.ToString()));
            }

            return texto.ToString();
        }

        public static string ArmarEgresosPorGrupo(IReadOnlyList<MovimientoDto> egresos, DateTime desde, DateTime hasta)
        {
            var total = egresos.Sum(e => e.Monto);
            var grupos = egresos.GroupBy(e => e.Grupo).OrderBy(g => g.Key).ToList();

            var texto = new StringBuilder();
            Fila(texto, $"Egresos por grupo del {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy} (sin los anulados)");
            texto.AppendLine();

            Fila(texto, "Resumen");
            Fila(texto, "Grupo", "Egresos", "Total", "% del total");
            foreach (var grupo in grupos)
            {
                var subtotal = grupo.Sum(e => e.Monto);
                Fila(texto, grupo.Key, grupo.Count().ToString(), subtotal.ToString("C2"),
                     (total == 0 ? 0 : subtotal / total).ToString("P1"));
            }
            Fila(texto, "Total", egresos.Count.ToString(), total.ToString("C2"), egresos.Count == 0 ? "" : 1m.ToString("P1"));

            foreach (var grupo in grupos)
            {
                texto.AppendLine();
                Fila(texto, grupo.Key);
                Fila(texto, "Fecha", "Motivo", "Medio de pago", "Monto", "Usuario");
                foreach (var e in grupo)
                    Fila(texto, e.Fecha.ToString("dd/MM/yyyy HH:mm"), e.Clasificacion, e.MedioPago, e.Monto.ToString("C2"), e.Usuario);
                Fila(texto, $"Subtotal {grupo.Key}", "", "", grupo.Sum(e => e.Monto).ToString("C2"), "");
            }

            return texto.ToString();
        }

        private static void Fila(StringBuilder texto, params string?[] celdas) => Fila(texto, (IEnumerable<string?>)celdas);

        private static void Fila(StringBuilder texto, IEnumerable<string?> celdas) =>
            texto.AppendLine(string.Join(Separador, celdas.Select(Escapar)));

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
