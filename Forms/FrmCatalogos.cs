using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Medios de pago, motivos y grupos de egreso: el administrador los agrega, renombra y desactiva.
    public partial class FrmCatalogos : Form
    {
        private readonly CatalogoController controlador;
        private List<ItemCatalogoDTO> items = new List<ItemCatalogoDTO>();
        private int idSeleccionado;
        private string valoresGuardados = string.Empty;

        public FrmCatalogos()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new CatalogoController();
            cmbCatalogo.FormattingEnabled = true;
            cmbCatalogo.Format += cmbCatalogo_Format;
        }

        private TipoCatalogo Tipo
        {
            get { return cmbCatalogo.SelectedItem is TipoCatalogo tipo ? tipo : TipoCatalogo.MedioPago; }
        }

        // "medio de pago", "motivo de egreso" o "grupo de egreso".
        private string NombreDelItem
        {
            get
            {
                switch (Tipo)
                {
                    case TipoCatalogo.MedioPago:
                        return "medio de pago";
                    case TipoCatalogo.MotivoEgreso:
                        return "motivo de egreso";
                    default:
                        return "grupo de egreso";
                }
            }
        }

        private void FrmCatalogos_Load(object? sender, EventArgs e)
        {
            foreach (TipoCatalogo tipo in Enum.GetValues<TipoCatalogo>())
            {
                cmbCatalogo.Items.Add(tipo);
            }
            cmbCatalogo.SelectedIndex = 0;
        }

        private void FrmCatalogos_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!Formulario.ConfirmarDescartarCambios(HayCambiosSinGuardar()))
            {
                e.Cancel = true;
            }
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void cmbCatalogo_Format(object? sender, ListControlConvertEventArgs e)
        {
            e.Value = TextoEnum.Texto(e.ListItem);
        }

        private void cmbCatalogo_SelectedIndexChanged(object? sender, EventArgs e)
        {
            lblDgvItems.Text = Tipo.Texto();
            CargarGrilla();
            LimpiarFormulario();
        }

        private void CargarGrilla()
        {
            try
            {
                items = controlador.ObtenerItems(Tipo);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
            MostrarGrilla();
        }

        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            MostrarGrilla();
        }

        private void MostrarGrilla()
        {
            List<ItemCatalogoDTO> visibles = new List<ItemCatalogoDTO>();
            foreach (ItemCatalogoDTO item in items)
            {
                if (Busqueda.Coincide(txtBuscar.Text, item.Nombre, item.Estado))
                {
                    visibles.Add(item);
                }
            }

            dgvItems.MensajeSinFilas(items.Count == 0
                ? "Todavía no hay ítems. Escriba el nombre y toque Guardar."
                : "Ningún ítem coincide con la búsqueda.");
            dgvItems.DataSource = null;
            dgvItems.DataSource = visibles;
            dgvItems.MostrarColumnas(("Nombre", "Nombre"), ("Estado", "Estado"));
            dgvItems.QuitarSeleccion();
        }

        private void dgvItems_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvItems.Rows)
            {
                if (fila.DataBoundItem is ItemCatalogoDTO item && !item.Activo)
                {
                    Tema.AtenuarFila(fila);
                }
            }
        }

        private void dgvItems_SelectionChanged(object? sender, EventArgs e)
        {
            ItemCatalogoDTO? seleccionado = dgvItems.Seleccionado<ItemCatalogoDTO>();
            btnEstado.Text = seleccionado != null && !seleccionado.Activo ? "Activar" : "Desactivar";
        }

        private void dgvItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditar_Click(sender, e);
            }
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            ItemCatalogoDTO? seleccionado = dgvItems.Seleccionado<ItemCatalogoDTO>();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un ítem de la grilla para renombrarlo.");
                return;
            }
            if (!Formulario.ConfirmarDescartarCambios(HayCambiosSinGuardar()))
            {
                return;
            }

            idSeleccionado = seleccionado.Id;
            txtNombre.Text = seleccionado.Nombre;
            lblModo.Text = "Renombrando: " + seleccionado.Nombre;
            tlpBotones.SetColumnSpan(btnGuardar, 1);
            btnCancelarEdicion.Visible = true;
            valoresGuardados = Formulario.Valores(tlpFormulario);
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            ItemCatalogoDTO dto = new ItemCatalogoDTO();
            dto.Id = idSeleccionado;
            dto.Tipo = Tipo;
            dto.Nombre = txtNombre.Text;

            try
            {
                bool eraEdicion = idSeleccionado != 0;
                ItemCatalogoDTO guardado = controlador.Guardar(dto);
                CargarGrilla();
                LimpiarFormulario();
                dgvItems.SeleccionarFila("Nombre", guardado.Nombre);
                Aviso.Mostrar(lblAviso, eraEdicion ? "Ítem renombrado." : "Ítem agregado.");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void btnCancelarEdicion_Click(object? sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void btnEstado_Click(object? sender, EventArgs e)
        {
            ItemCatalogoDTO? seleccionado = dgvItems.Seleccionado<ItemCatalogoDTO>();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un ítem de la grilla para activarlo o desactivarlo.");
                return;
            }
            if (seleccionado.Activo && !Mensajes.Confirmar("¿Desactivar \"" + seleccionado.Nombre
                + "\"? No se va a poder elegir en movimientos nuevos; los ya registrados no cambian."))
            {
                return;
            }

            try
            {
                controlador.CambiarEstado(Tipo, seleccionado.Id, !seleccionado.Activo);
                CargarGrilla();
                dgvItems.SeleccionarFila("Nombre", seleccionado.Nombre);
                Aviso.Mostrar(lblAviso, seleccionado.Activo ? "Ítem desactivado." : "Ítem activado.");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void LimpiarFormulario()
        {
            idSeleccionado = 0;
            txtNombre.Text = string.Empty;
            lblModo.Text = "Nuevo " + NombreDelItem;
            tlpBotones.SetColumnSpan(btnGuardar, 2);
            btnCancelarEdicion.Visible = false;
            valoresGuardados = Formulario.Valores(tlpFormulario);
        }

        private bool HayCambiosSinGuardar()
        {
            return Formulario.Valores(tlpFormulario) != valoresGuardados;
        }
    }
}
