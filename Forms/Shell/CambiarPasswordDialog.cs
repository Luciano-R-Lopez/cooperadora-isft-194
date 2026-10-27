using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms.Shell
{
    /// <summary>El usuario logueado cambia su propia contraseña (cualquier rol).</summary>
    public class CambiarPasswordDialog : Form
    {
        private readonly UsuarioController _controller;
        private readonly TextBox _txtActual = Ui.Campo(20, 40, 300);
        private readonly TextBox _txtNueva = Ui.Campo(20, 100, 300);
        private readonly TextBox _txtConfirmacion = Ui.Campo(20, 160, 300);

        public CambiarPasswordDialog() : this(new UsuarioController()) { }

        public CambiarPasswordDialog(UsuarioController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Tema.Aplicar(this);
        }

        private void ConstruirInterfaz()
        {
            Text = "Cambiar mi contraseña";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(340, 260);

            foreach (var campo in new[] { _txtActual, _txtNueva, _txtConfirmacion })
                campo.UseSystemPasswordChar = true;

            var btnGuardar = new Button { Text = "Guardar", Location = new Point(150, 210), Size = new Size(90, 32) };
            btnGuardar.Click += (_, _) => Guardar();

            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(250, 210),
                Size = new Size(70, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = btnGuardar;
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Contraseña actual", 20, 20), _txtActual,
                Ui.Etiqueta("Nueva contraseña (6 a 64 caracteres)", 20, 80), _txtNueva,
                Ui.Etiqueta("Repetir la nueva contraseña", 20, 140), _txtConfirmacion,
                btnGuardar, btnCancelar
            });
        }

        private void Guardar()
        {
            try
            {
                _controller.CambiarMiPassword(new CambioPasswordDto(_txtActual.Text, _txtNueva.Text, _txtConfirmacion.Text));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Ui.Info("Contraseña cambiada.");
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
