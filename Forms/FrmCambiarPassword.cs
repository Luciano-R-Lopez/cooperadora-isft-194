using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Cualquier usuario cambia su propia contraseña: escribe la actual y la nueva dos veces.
    public partial class FrmCambiarPassword : Form
    {
        private readonly UsuarioController controlador;

        public FrmCambiarPassword()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new UsuarioController();
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            CambioPasswordDTO dto = new CambioPasswordDTO();
            dto.Actual = txtActual.Text;
            dto.Nueva = txtNueva.Text;
            dto.Confirmacion = txtConfirmacion.Text;

            try
            {
                controlador.CambiarMiPassword(dto);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return;
            }

            Mensajes.Informacion("La contraseña se cambió. Úsela la próxima vez que ingrese.");
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
