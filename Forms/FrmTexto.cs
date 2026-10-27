using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Pide un texto (por ejemplo, el motivo de una anulación). No valida: eso lo hace el dominio.
    public partial class FrmTexto : Form
    {
        public FrmTexto()
        {
            InitializeComponent();
            Tema.Aplicar(this);
        }

        // Devuelve el texto escrito, o null si se canceló.
        public static string? Pedir(IWin32Window duenio, string titulo, string pregunta, string textoAceptar)
        {
            using (FrmTexto dialogo = new FrmTexto())
            {
                dialogo.Text = titulo;
                dialogo.lblDatoPregunta.Text = pregunta;
                dialogo.btnAceptar.Text = textoAceptar;
                return dialogo.ShowDialog(duenio) == DialogResult.OK ? dialogo.txtTexto.Text : null;
            }
        }
    }
}
