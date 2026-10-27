using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Registro de quién hizo qué y cuándo, con filtros por fecha, acción y texto.
    public partial class FrmAuditoria : Form
    {
        private readonly AuditoriaController controlador;
        private List<AuditoriaDTO> registros = new List<AuditoriaDTO>();
        private bool cargando;

        public FrmAuditoria()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new AuditoriaController();
        }

        private void FrmAuditoria_Load(object? sender, EventArgs e)
        {
            cargando = true;
            cmbAccion.Items.Add(new Opcion<AccionAuditoria?>("Todas", null));
            foreach (AccionAuditoria accion in Enum.GetValues<AccionAuditoria>())
            {
                cmbAccion.Items.Add(new Opcion<AccionAuditoria?>(accion.Texto(), accion));
            }
            cmbAccion.SelectedIndex = 0;
            dtpDesde.Value = DateTime.Today.AddDays(-30);
            dtpHasta.Value = DateTime.Today;
            cargando = false;
            CargarRegistros();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void chkDesde_CheckedChanged(object? sender, EventArgs e)
        {
            dtpDesde.Enabled = chkDesde.Checked;
            CargarRegistros();
        }

        private void chkHasta_CheckedChanged(object? sender, EventArgs e)
        {
            dtpHasta.Enabled = chkHasta.Checked;
            CargarRegistros();
        }

        private void Filtro_Cambio(object? sender, EventArgs e)
        {
            CargarRegistros();
        }

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            MostrarGrilla();
        }

        private void CargarRegistros()
        {
            if (cargando)
            {
                return;
            }

            AuditoriaFiltroDTO filtro = new AuditoriaFiltroDTO();
            filtro.Desde = chkDesde.Checked ? dtpDesde.Value : null;
            filtro.Hasta = chkHasta.Checked ? dtpHasta.Value : null;
            filtro.Accion = (cmbAccion.SelectedItem as Opcion<AccionAuditoria?>)?.Valor;
            try
            {
                registros = controlador.Obtener(filtro);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            MostrarGrilla();
        }

        private void MostrarGrilla()
        {
            List<AuditoriaDTO> visibles = new List<AuditoriaDTO>();
            foreach (AuditoriaDTO registro in registros)
            {
                if (Busqueda.Coincide(txtBuscar.Text, registro.Usuario, registro.EntidadAfectada, registro.Detalle))
                {
                    visibles.Add(registro);
                }
            }

            dgvAuditoria.MensajeSinFilas("No hay registros con esos filtros.");
            dgvAuditoria.DataSource = null;
            dgvAuditoria.DataSource = visibles;
            dgvAuditoria.MostrarColumnas(("Fecha", "Fecha y hora"), ("Usuario", "Usuario"), ("Accion", "Acción"),
                ("EntidadAfectada", "Dato"), ("EntidadId", "Id"), ("Detalle", "Detalle"));
            dgvAuditoria.FormatoFecha("Fecha", "dd/MM/yyyy HH:mm:ss");
            dgvAuditoria.Columna("EntidadId").FillWeight = 30;
            dgvAuditoria.Columna("Detalle").FillWeight = 260;
            dgvAuditoria.MostrarEnumsConTexto();
            dgvAuditoria.QuitarSeleccion();
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            ExportarCsv.Grilla(dgvAuditoria, "Auditoría");
        }
    }
}
