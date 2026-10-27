using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>Utilidades de interfaz compartidas por todas las vistas.</summary>
    public static class Ui
    {
        private const string Titulo = "Cooperadora ISFT 194";

        public static void Error(string mensaje) =>
            Mensaje.Mostrar(Titulo, mensaje, Mensaje.Tipo.Advertencia);

        /// <summary>Error de programación (no de negocio): se muestra el mensaje técnico para poder reportarlo.</summary>
        public static void ErrorInesperado(Exception excepcion)
        {
            var texto = $"Ocurrió un error inesperado: {excepcion.Message}\n\nEl detalle quedó guardado en:\n{RegistroErrores.Archivo}";
            try
            {
                Mensaje.Mostrar(Titulo, texto, Mensaje.Tipo.Error);
            }
            catch (Exception)
            {
                // Si falla la ventana con el tema, el aviso igual tiene que llegar al usuario.
                MessageBox.Show(texto, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Info(string mensaje) =>
            Mensaje.Mostrar(Titulo, mensaje, Mensaje.Tipo.Info);

        public static bool Confirmar(string mensaje) =>
            Mensaje.Mostrar(Titulo, mensaje, Mensaje.Tipo.Pregunta) == DialogResult.Yes;

        /// <summary>
        /// Contenedor que ocupa todo el espacio y mantiene el contenido (de tamaño fijo) centrado a lo ancho,
        /// así los formularios no quedan pegados a la izquierda en pantalla completa.
        /// </summary>
        public static Panel Centrado(Control contenido, int margenSuperior = 30)
        {
            var contenedor = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            contenedor.Controls.Add(contenido);

            void Ubicar() =>
                contenido.Location = new Point(Math.Max(0, (contenedor.ClientSize.Width - contenido.Width) / 2), margenSuperior);

            contenedor.Resize += (_, _) => Ubicar();
            contenedor.HandleCreated += (_, _) => Ubicar();
            return contenedor;
        }

        /// <summary>
        /// Carga las opciones de un desplegable y elige una. Usa Items y no DataSource: con DataSource las
        /// opciones recién aparecen cuando el control está en pantalla, y elegir una antes falla.
        /// </summary>
        public static void Llenar<T>(ComboBox combo, IEnumerable<T> opciones, int indice = 0)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            foreach (var opcion in opciones)
                combo.Items.Add(opcion!);
            combo.EndUpdate();

            combo.SelectedIndex = combo.Items.Count == 0 ? -1 : Math.Clamp(indice, 0, combo.Items.Count - 1);
        }

        /// <summary>Etiqueta de un campo. Va 3 px más arriba para no taparle el borde superior al campo de abajo.</summary>
        public static Label Etiqueta(string texto, int x, int y) =>
            new() { Text = texto, Location = new Point(x, y - 3), AutoSize = true };

        public static TextBox Campo(int x, int y, int ancho) =>
            new() { Location = new Point(x, y), Size = new Size(ancho, 27) };

        /// <summary>Grilla de solo lectura, con selección de fila completa y columnas manuales.</summary>
        public static DataGridView CrearGrilla() =>
            new()
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Tema.Superficie
            };

        public static void AgregarColumna(DataGridView grilla, string titulo, string propiedad, string? formato = null)
        {
            var columna = new DataGridViewTextBoxColumn
            {
                HeaderText = titulo,
                DataPropertyName = propiedad
            };

            if (formato is not null)
                columna.DefaultCellStyle.Format = formato;

            grilla.Columns.Add(columna);
        }
    }
}
