namespace CooperadoraISFT94.Views.Common
{
    /// <summary>Utilidades de interfaz compartidas por todas las vistas.</summary>
    public static class Ui
    {
        private const string Titulo = "Cooperadora ISFT 94";

        public static void Error(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        /// <summary>Error de programación (no de negocio): se muestra el mensaje técnico para poder reportarlo.</summary>
        public static void ErrorInesperado(Exception excepcion) =>
            MessageBox.Show("Ocurrió un error inesperado: " + excepcion.Message, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void Info(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static bool Confirmar(string mensaje) =>
            MessageBox.Show(mensaje, Titulo, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public static Label Etiqueta(string texto, int x, int y) =>
            new() { Text = texto, Location = new Point(x, y), AutoSize = true };

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
                BackgroundColor = SystemColors.Window
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
