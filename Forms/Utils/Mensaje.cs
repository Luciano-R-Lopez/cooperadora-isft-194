namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>
    /// Ventana de aviso con los colores del <see cref="Tema"/>, en lugar del MessageBox de Windows (siempre claro).
    /// Se usa a través de <see cref="Ui"/>: Error, Info, Confirmar y ErrorInesperado.
    /// </summary>
    internal sealed class Mensaje : Form
    {
        public enum Tipo { Info, Advertencia, Error, Pregunta }

        private const int AnchoTexto = 420;

        private Mensaje(string titulo, string texto, Tipo tipo)
        {
            Text = titulo;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = Form.ActiveForm is null ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = Form.ActiveForm is null;
            KeyPreview = true;

            var (simbolo, color) = tipo switch
            {
                Tipo.Info => ("i", Tema.Primario),
                Tipo.Advertencia => ("!", ColorTranslator.FromHtml("#B08A3E")),
                Tipo.Error => ("✕", Tema.Peligro),
                _ => ("?", Tema.Primario)
            };

            var icono = Tema.ConColoresPropios(new Label
            {
                Text = simbolo,
                Size = new Size(36, 36),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font(Font.FontFamily, 13f, FontStyle.Bold)
            });

            var cuerpo = new Label
            {
                Text = texto,
                AutoSize = true,
                MaximumSize = new Size(AnchoTexto, 0),
                Location = new Point(72, 26)
            };
            Controls.Add(cuerpo);
            var altoTexto = Math.Max(36, cuerpo.PreferredSize.Height);

            var botones = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Height = 40,
                Width = AnchoTexto + 72,
                Location = new Point(0, 26 + altoTexto + 18),
                Padding = new Padding(0, 0, 14, 0)
            };

            if (tipo == Tipo.Pregunta)
            {
                var no = Tema.Secundario(new Button { Text = "No", Size = new Size(90, 32), DialogResult = DialogResult.No });
                var si = new Button { Text = "Sí", Size = new Size(90, 32), DialogResult = DialogResult.Yes };
                botones.Controls.AddRange(new Control[] { no, si });
                AcceptButton = si;
                CancelButton = no;
            }
            else
            {
                var aceptar = new Button { Text = "Aceptar", Size = new Size(100, 32), DialogResult = DialogResult.OK };
                botones.Controls.Add(aceptar);
                AcceptButton = aceptar;
                CancelButton = aceptar;
            }

            Controls.Add(icono);
            Controls.Add(botones);
            ClientSize = new Size(Math.Max(320, Math.Min(AnchoTexto, cuerpo.PreferredSize.Width) + 100), botones.Bottom + 10);
            botones.Width = ClientSize.Width;

            // Ctrl+C copia el texto, como en los mensajes de Windows (útil para reportar errores).
            KeyDown += (_, e) =>
            {
                if (e.Control && e.KeyCode == Keys.C)
                    Clipboard.SetText(texto);
            };

            Tema.Aplicar(this);
        }

        public static DialogResult Mostrar(string titulo, string texto, Tipo tipo)
        {
            using var mensaje = new Mensaje(titulo, texto, tipo);
            return Form.ActiveForm is Form duenio && duenio != mensaje
                ? mensaje.ShowDialog(duenio)
                : mensaje.ShowDialog();
        }
    }
}
