using CooperadoraISFT94.Common;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Elige qué cuotas pendientes se cobran juntas (un solo ingreso y un solo recibo) y con qué medio de pago.
    // No guarda nada: quien lo abre registra el cobro con lo elegido.
    public partial class FrmCobrarCuotas : Form
    {
        // Para que el diseñador de Visual Studio pueda abrir el formulario.
        public FrmCobrarCuotas()
        {
            InitializeComponent();
            Tema.Aplicar(this);
        }

        // responsable: quien paga por el socio, para ofrecer "Paga su responsable"; vacío si no tiene.
        // variosSocios: las cuotas son de distintos socios (un responsable que paga por sus socios a cargo).
        public FrmCobrarCuotas(string titulo, List<CuotaDTO> pendientes, List<ItemCatalogoDTO> medios, string responsable,
            bool variosSocios)
            : this()
        {
            lblTituloCobro.Text = titulo;
            lstCuotas.DisplayMember = variosSocios ? "DescripcionConSocio" : "Descripcion";
            foreach (CuotaDTO cuota in pendientes.OrderBy(c => c.Socio).ThenBy(c => c.Anio))
            {
                lstCuotas.Items.Add(cuota, true);
            }
            foreach (ItemCatalogoDTO medio in medios)
            {
                cmbMedio.Items.Add(medio);
            }
            if (cmbMedio.Items.Count > 0)
            {
                cmbMedio.SelectedIndex = 0;
            }

            // Si tiene responsable, lo habitual es que pague él.
            chkPagaResponsable.Visible = responsable.Length > 0;
            chkPagaResponsable.Checked = responsable.Length > 0;
            chkPagaResponsable.Text = "Paga su responsable: " + responsable;
            MostrarTotal(CuotasElegidas());
        }

        public List<int> CuotaIds
        {
            get { return CuotasElegidas().Select(c => c.Id).ToList(); }
        }

        public int MedioPagoId
        {
            get { return cmbMedio.SelectedItem is ItemCatalogoDTO medio ? medio.Id : 0; }
        }

        public bool PagaResponsable
        {
            get { return chkPagaResponsable.Visible && chkPagaResponsable.Checked; }
        }

        private List<CuotaDTO> CuotasElegidas()
        {
            return lstCuotas.CheckedItems.Cast<CuotaDTO>().ToList();
        }

        // ItemCheck avisa antes de que cambie el tilde: se calcula con el cambio que está por hacerse.
        private void lstCuotas_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            List<CuotaDTO> elegidas = CuotasElegidas();
            CuotaDTO cuota = (CuotaDTO)lstCuotas.Items[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                elegidas.Add(cuota);
            }
            else
            {
                elegidas.Remove(cuota);
            }
            MostrarTotal(elegidas);
        }

        private void MostrarTotal(List<CuotaDTO> elegidas)
        {
            lblTotalCobro.Text = elegidas.Count == 0
                ? "Elija al menos una cuota."
                : "Total a cobrar: " + Formato.Monto(elegidas.Sum(c => c.Monto)) + " (" + elegidas.Count
                    + (elegidas.Count == 1 ? " cuota)" : " cuotas)");
        }

        private void btnCobrar_Click(object? sender, EventArgs e)
        {
            if (CuotasElegidas().Count == 0)
            {
                Mensajes.Advertencia("Elija al menos una cuota para cobrar.");
                return;
            }
            if (MedioPagoId == 0)
            {
                Mensajes.Advertencia("Elija el medio de pago.");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
