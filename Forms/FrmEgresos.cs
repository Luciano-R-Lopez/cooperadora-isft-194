using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Gastos de la caja. Cada egreso tiene un motivo y, aparte, el grupo al que corresponde el gasto.
    public partial class FrmEgresos : Form
    {
        private readonly EgresoController egresos;
        private readonly CajaController cajas;
        private readonly CatalogoController catalogos;
        private string valoresGuardados = string.Empty;

        public FrmEgresos()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            egresos = new EgresoController();
            cajas = new CajaController();
            catalogos = new CatalogoController();
        }

        private void FrmEgresos_Load(object? sender, EventArgs e)
        {
            try
            {
                Tema.MostrarEstadoCaja(lblEstadoCaja, cajas.ObtenerActiva() != null);
                Llenar(cmbMotivo, catalogos.ObtenerMotivosEgreso());
                Llenar(cmbGrupo, catalogos.ObtenerGruposEgreso(false));
                Llenar(cmbMedio, catalogos.ObtenerMediosPago(false));
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            LimpiarFormulario();
        }

        private void FrmEgresos_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!Formulario.ConfirmarDescartarCambios(Formulario.Valores(tlpFormulario) != valoresGuardados))
            {
                e.Cancel = true;
            }
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private static void Llenar(ComboBox combo, List<ItemCatalogoDTO> items)
        {
            foreach (ItemCatalogoDTO item in items)
            {
                combo.Items.Add(item);
            }
        }

        private void btnRegistrar_Click(object? sender, EventArgs e)
        {
            EgresoDTO dto = new EgresoDTO();
            dto.Monto = numMonto.Value;
            dto.MotivoEgresoId = cmbMotivo.SelectedItem is ItemCatalogoDTO motivo ? motivo.Id : 0;
            dto.GrupoEgresoId = cmbGrupo.SelectedItem is ItemCatalogoDTO grupo ? grupo.Id : 0;
            dto.MedioPagoId = cmbMedio.SelectedItem is ItemCatalogoDTO medio ? medio.Id : 0;

            try
            {
                EgresoDTO registrado = egresos.Registrar(dto);
                LimpiarFormulario();
                Aviso.Mostrar(lblAviso, "Egreso registrado: " + registrado.Motivo + " (" + registrado.Grupo + ") por "
                    + Formato.Monto(registrado.Monto) + ".");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            numMonto.Value = 0;
            foreach (ComboBox combo in new[] { cmbMotivo, cmbGrupo, cmbMedio })
            {
                if (combo.Items.Count > 0)
                {
                    combo.SelectedIndex = 0;
                }
            }
            valoresGuardados = Formulario.Valores(tlpFormulario);
            numMonto.Focus();
        }
    }
}
