namespace CooperadoraISFT94.Views.Common
{
    /// <summary>Diálogo simple que pide un texto (por ejemplo, el motivo de una anulación). No valida: eso lo hace el dominio.</summary>
    public class DialogoTexto : Form
    {
        private readonly TextBox _txtValor = new();

        public string Valor => _txtValor.Text;

        private DialogoTexto(string titulo, string pregunta, string textoAceptar)
        {
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(420, 150);

            var etiqueta = new Label { Text = pregunta, Location = new Point(20, 15), Size = new Size(380, 40) };

            _txtValor.Location = new Point(20, 60);
            _txtValor.Size = new Size(380, 27);

            var btnAceptar = new Button
            {
                Text = textoAceptar,
                Location = new Point(190, 102),
                Size = new Size(120, 32),
                DialogResult = DialogResult.OK
            };
            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(320, 102),
                Size = new Size(80, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = btnAceptar;
            CancelButton = btnCancelar;
            Controls.AddRange(new Control[] { etiqueta, _txtValor, btnAceptar, btnCancelar });
            Tema.Aplicar(this);
        }

        /// <summary>Muestra el diálogo; devuelve el texto ingresado o null si se canceló.</summary>
        public static string? Pedir(IWin32Window? duenio, string titulo, string pregunta, string textoAceptar = "Aceptar")
        {
            using var dialogo = new DialogoTexto(titulo, pregunta, textoAceptar);
            return dialogo.ShowDialog(duenio) == DialogResult.OK ? dialogo.Valor : null;
        }
    }
}
