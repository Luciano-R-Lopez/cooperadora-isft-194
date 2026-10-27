using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Donaciones y subsidios. Las cuotas sociales se cobran desde la cuota, para que el ingreso quede ligado a ella.
    public partial class FrmIngresos : Form
    {
        private readonly IngresoController ingresos;
        private readonly CajaController cajas;
        private readonly CatalogoController catalogos;
        private string valoresGuardados = string.Empty;

        public FrmIngresos()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            ingresos = new IngresoController();
            cajas = new CajaController();
            catalogos = new CatalogoController();
            cmbCategoria.FormattingEnabled = true;
            cmbCategoria.Format += cmbCategoria_Format;
        }

        private void FrmIngresos_Load(object? sender, EventArgs e)
        {
            cmbCategoria.Items.Add(CategoriaIngreso.Donacion);
            cmbCategoria.Items.Add(CategoriaIngreso.Subsidio);
            try
            {
                Tema.MostrarEstadoCaja(lblEstadoCaja, cajas.ObtenerActiva() != null);
                foreach (ItemCatalogoDTO medio in catalogos.ObtenerMediosPago(false))
                {
                    cmbMedio.Items.Add(medio);
                }
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            LimpiarFormulario();
        }

        private void FrmIngresos_FormClosing(object? sender, FormClosingEventArgs e)
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

        private void cmbCategoria_Format(object? sender, ListControlConvertEventArgs e)
        {
            e.Value = TextoEnum.Texto(e.ListItem);
        }

        private void btnRegistrar_Click(object? sender, EventArgs e)
        {
            IngresoDTO dto = new IngresoDTO();
            dto.Monto = numMonto.Value;
            dto.CategoriaIngreso = cmbCategoria.SelectedItem is CategoriaIngreso categoria ? categoria : CategoriaIngreso.Donacion;
            dto.MedioPagoId = cmbMedio.SelectedItem is ItemCatalogoDTO medio ? medio.Id : 0;

            try
            {
                IngresoDTO registrado = ingresos.Registrar(dto);
                LimpiarFormulario();
                Aviso.Mostrar(lblAviso, "Ingreso registrado: " + registrado.CategoriaIngreso.Texto() + " por " + Formato.Monto(registrado.Monto) + ".");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            numMonto.Value = 0;
            if (cmbCategoria.Items.Count > 0)
            {
                cmbCategoria.SelectedIndex = 0;
            }
            if (cmbMedio.Items.Count > 0)
            {
                cmbMedio.SelectedIndex = 0;
            }
            valoresGuardados = Formulario.Valores(tlpFormulario);
            numMonto.Focus();
        }
    }
}
