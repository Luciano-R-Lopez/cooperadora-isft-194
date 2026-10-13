using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;

namespace CooperadoraISFT94.Views.Auth
{
    public class LoginForm : Form
    {
        private readonly AuthController _controller;

        private readonly TextBox _txtUsuario = new();
        private readonly TextBox _txtPassword = new();
        private readonly Button _btnIngresar = new();
        private readonly Label _lblError = new();

        public LoginForm() : this(new AuthController()) { }

        public LoginForm(AuthController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Text = "Cooperadora ISFT 94 - Iniciar sesión";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(340, 260);

            var titulo = new Label
            {
                Text = "Cooperadora ISFT 94",
                Font = new Font(Font.FontFamily, 14f, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 15),
                Size = new Size(300, 35)
            };

            var lblUsuario = new Label { Text = "Usuario", Location = new Point(20, 65), AutoSize = true };
            _txtUsuario.Location = new Point(20, 85);
            _txtUsuario.Size = new Size(300, 27);

            var lblPassword = new Label { Text = "Contraseña", Location = new Point(20, 120), AutoSize = true };
            _txtPassword.Location = new Point(20, 140);
            _txtPassword.Size = new Size(300, 27);
            _txtPassword.UseSystemPasswordChar = true;

            _lblError.Location = new Point(20, 175);
            _lblError.Size = new Size(300, 20);
            _lblError.ForeColor = Color.Firebrick;

            _btnIngresar.Text = "Ingresar";
            _btnIngresar.Location = new Point(20, 205);
            _btnIngresar.Size = new Size(300, 35);
            _btnIngresar.Click += (_, _) => Ingresar();

            AcceptButton = _btnIngresar;
            Controls.AddRange(new Control[]
            {
                titulo, lblUsuario, _txtUsuario, lblPassword, _txtPassword, _lblError, _btnIngresar
            });
        }

        private void Ingresar()
        {
            _lblError.Text = string.Empty;

            try
            {
                _controller.IniciarSesion(_txtUsuario.Text, _txtPassword.Text);
            }
            catch (NegocioException ex)
            {
                _lblError.Text = ex.Message;
                _txtPassword.Clear();
                _txtPassword.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
