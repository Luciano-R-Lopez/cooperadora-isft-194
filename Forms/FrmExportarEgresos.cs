using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Pide el período de los egresos a exportar. Por defecto, desde el 1° de enero hasta hoy.
    public partial class FrmExportarEgresos : Form
    {
        public FrmExportarEgresos()
        {
            InitializeComponent();
            Tema.Aplicar(this);
        }

        public DateTime Desde
        {
            get { return dtpDesde.Value; }
        }

        public DateTime Hasta
        {
            get { return dtpHasta.Value; }
        }

        private void FrmExportarEgresos_Load(object? sender, EventArgs e)
        {
            dtpDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpHasta.Value = DateTime.Today;
        }
    }
}
