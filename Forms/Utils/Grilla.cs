using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Forms.Utils
{
    internal static class Grilla
    {
        public static DataGridViewColumn Columna(this DataGridView grilla, string nombre)
        {
            return grilla.Columns[nombre]
                ?? throw new InvalidOperationException("La grilla " + grilla.Name + " no tiene la columna " + nombre + ".");
        }

        // Muestra solo las columnas indicadas, en ese orden y con esos títulos (nombre de la propiedad del DTO, título).
        // Las columnas se arman una sola vez: si se dejaran generar solas, Windows las reordena al mostrar la grilla.
        public static void MostrarColumnas(this DataGridView grilla, params (string Nombre, string Titulo)[] columnas)
        {
            if (!grilla.AutoGenerateColumns)
            {
                return;
            }

            object? datos = grilla.DataSource;
            grilla.DataSource = null;
            grilla.AutoGenerateColumns = false;
            grilla.Columns.Clear();
            foreach ((string Nombre, string Titulo) columna in columnas)
            {
                DataGridViewTextBoxColumn nueva = new DataGridViewTextBoxColumn();
                nueva.Name = columna.Nombre;
                nueva.DataPropertyName = columna.Nombre;
                nueva.HeaderText = columna.Titulo;
                nueva.SortMode = DataGridViewColumnSortMode.NotSortable;
                grilla.Columns.Add(nueva);
            }
            grilla.DataSource = datos;
        }

        public static void FormatoMoneda(this DataGridView grilla, params string[] columnas)
        {
            foreach (string nombre in columnas)
            {
                DataGridViewColumn columna = grilla.Columna(nombre);
                columna.DefaultCellStyle.Format = Formato.Moneda;
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        public static void FormatoFecha(this DataGridView grilla, string columna, string formato)
        {
            grilla.Columna(columna).DefaultCellStyle.Format = formato;
        }

        // Muestra los valores de enumeraciones con su texto ([Description]): "Cuota social", "Modificación".
        public static void MostrarEnumsConTexto(this DataGridView grilla)
        {
            grilla.CellFormatting -= FormatearEnum;
            grilla.CellFormatting += FormatearEnum;
        }

        public static void QuitarSeleccion(this DataGridView grilla)
        {
            grilla.ClearSelection();
            grilla.CurrentCell = null;
        }

        public static T? Seleccionado<T>(this DataGridView grilla) where T : class
        {
            return grilla.CurrentRow?.DataBoundItem as T;
        }

        // Selecciona la fila cuyo DTO tiene ese valor en la propiedad indicada (aunque no se muestre como columna).
        public static void SeleccionarFila(this DataGridView grilla, string propiedad, string valor)
        {
            foreach (DataGridViewRow fila in grilla.Rows)
            {
                object? elemento = fila.DataBoundItem;
                string? valorFila = elemento?.GetType().GetProperty(propiedad)?.GetValue(elemento)?.ToString();
                if (valor.Equals(valorFila, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (DataGridViewCell celda in fila.Cells)
                    {
                        if (celda.Visible)
                        {
                            grilla.CurrentCell = celda;
                            break;
                        }
                    }
                    fila.Selected = true;
                    return;
                }
            }
        }

        // El mensaje se guarda y se dibuja mientras la grilla no tenga filas.
        public static void MensajeSinFilas(this DataGridView grilla, string mensaje)
        {
            grilla.AccessibleDescription = mensaje;
            grilla.Paint -= DibujarMensajeSinFilas;
            grilla.Paint += DibujarMensajeSinFilas;
            grilla.Resize -= Redibujar;
            grilla.Resize += Redibujar;
            grilla.Invalidate();
        }

        private static void FormatearEnum(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is Enum valor)
            {
                e.Value = valor.Texto();
                e.FormattingApplied = true;
            }
        }

        private static void DibujarMensajeSinFilas(object? sender, PaintEventArgs e)
        {
            if (sender is not DataGridView grilla || grilla.Rows.Count > 0 || string.IsNullOrEmpty(grilla.AccessibleDescription))
            {
                return;
            }
            int arriba = grilla.ColumnHeadersVisible ? grilla.ColumnHeadersHeight : 0;
            Rectangle area = new Rectangle(20, arriba + 20, Math.Max(grilla.Width - 40, 0), 80);
            Font fuente = grilla.DefaultCellStyle.Font ?? grilla.Font;
            TextRenderer.DrawText(e.Graphics, grilla.AccessibleDescription, fuente, area, Tema.TextoSecundario,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.WordBreak);
        }

        private static void Redibujar(object? sender, EventArgs e)
        {
            if (sender is Control control)
            {
                control.Invalidate();
            }
        }
    }
}
