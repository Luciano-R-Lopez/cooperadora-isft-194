using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Aviso con los colores del tema, en lugar del MessageBox de Windows (que siempre se ve claro).
    // Se usa a través de Mensajes.
    public partial class FrmMensaje : Form
    {
        public FrmMensaje()
        {
            InitializeComponent();
            Tema.Aplicar(this);
        }

        // confirmar: true muestra Sí/No y devuelve true si se eligió Sí; false muestra solo Aceptar.
        public static bool Mostrar(string titulo, string texto, bool confirmar)
        {
            using (FrmMensaje mensaje = new FrmMensaje())
            {
                mensaje.Text = titulo;
                mensaje.lblDatoTexto.Text = texto;
                mensaje.btnCancelar.Visible = confirmar;
                mensaje.btnAceptar.Text = confirmar ? "Sí" : "Aceptar";
                mensaje.btnCancelar.Text = "No";

                Form? duenio = Form.ActiveForm;
                if (duenio == null || !duenio.Visible)
                {
                    mensaje.StartPosition = FormStartPosition.CenterScreen;
                    return mensaje.ShowDialog() == DialogResult.OK;
                }
                return mensaje.ShowDialog(duenio) == DialogResult.OK;
            }
        }
    }
}
