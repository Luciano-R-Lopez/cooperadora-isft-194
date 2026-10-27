using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms.Auth
{
    public class LoginForm : Form
    {
        private readonly AuthController _controller;

        private readonly TextBox _txtUsuario = new();
        private readonly TextBox _txtPassword = new();
        private readonly Button _btnIngresar = new();
        private readonly Label _lblError = new();
        private readonly Panel _tarjeta = new();

        public LoginForm() : this(new AuthController()) { }

        public LoginForm(AuthController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Tema.Aplicar(this);

            // Colores propios del login, después del tema para que no los pise.
            _lblError.ForeColor = Tema.PeligroTexto;
        }

        private void ConstruirInterfaz()
        {
            Text = "Cooperadora ISFT 194 - Iniciar sesión";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(800, 500);
            MinimumSize = new Size(420, 340);
            WindowState = FormWindowState.Maximized;

            // El formulario va en una tarjeta blanca que se mantiene centrada en la pantalla.
            Tema.ConColoresPropios(_tarjeta);
            _tarjeta.BackColor = Tema.Superficie;
            _tarjeta.BorderStyle = BorderStyle.FixedSingle;
            _tarjeta.Size = new Size(340, 260);
            Resize += (_, _) => CentrarTarjeta();
            Load += (_, _) => CentrarTarjeta();

            var titulo = Tema.ConColoresPropios(new Label
            {
                Text = "Cooperadora ISFT 194",
                ForeColor = Tema.Texto,
                Font = new Font(Font.FontFamily, 14f, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 15),
                Size = new Size(300, 35)
            });

            var lblUsuario = new Label { Text = "Usuario", Location = new Point(20, 65), AutoSize = true };
            _txtUsuario.Location = new Point(20, 85);
            _txtUsuario.Size = new Size(300, 27);

            var lblPassword = new Label { Text = "Contraseña", Location = new Point(20, 120), AutoSize = true };
            _txtPassword.Location = new Point(20, 140);
            _txtPassword.Size = new Size(300, 27);
            _txtPassword.UseSystemPasswordChar = true;

            _lblError.Location = new Point(20, 175);
            _lblError.Size = new Size(300, 20);

            _btnIngresar.Text = "Ingresar";
            _btnIngresar.Location = new Point(20, 205);
            _btnIngresar.Size = new Size(300, 35);
            _btnIngresar.Click += (_, _) => Ingresar();

            AcceptButton = _btnIngresar;
            _tarjeta.Controls.AddRange(new Control[]
            {
                titulo, lblUsuario, _txtUsuario, lblPassword, _txtPassword, _lblError, _btnIngresar
            });
            Controls.Add(_tarjeta);
        }

        private void CentrarTarjeta() =>
            _tarjeta.Location = new Point(
                Math.Max(0, (ClientSize.Width - _tarjeta.Width) / 2),
                Math.Max(0, (ClientSize.Height - _tarjeta.Height) / 2));

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
