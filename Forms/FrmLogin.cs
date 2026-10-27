using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly AuthController controlador;

        public FrmLogin()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new AuthController();
        }

        private void FrmLogin_Shown(object? sender, EventArgs e)
        {
            txtUsuario.Focus();
        }

        private void btnIngresar_Click(object? sender, EventArgs e)
        {
            LoginDTO login = new LoginDTO();
            login.NombreUsuario = txtUsuario.Text;
            login.Password = txtPassword.Text;

            try
            {
                controlador.IniciarSesion(login);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
