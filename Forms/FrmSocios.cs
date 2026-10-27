using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Inscripción y edición de socios. Dar de baja es una baja lógica: el socio conserva sus cuotas y pagos.
    public partial class FrmSocios : Form, IVuelveDeSubpantalla
    {
        private readonly SocioController controlador;
        private List<SocioDTO> socios = new List<SocioDTO>();
        private int idSeleccionado;
        private string valoresGuardados = string.Empty;

        public FrmSocios()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new SocioController();
            foreach (Categoria categoria in Enum.GetValues<Categoria>())
            {
                cmbCategoria.Items.Add(categoria);
            }
        }

        private void FrmSocios_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
            LimpiarFormulario();
        }

        private void FrmSocios_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!Formulario.ConfirmarDescartarCambios(HayCambiosSinGuardar()))
            {
                e.Cancel = true;
            }
        }

        public void AlVolverDeSubpantalla(Form subpantalla)
        {
            CargarGrilla();
        }

        private void btnVolver_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void CargarGrilla()
        {
            try
            {
                socios = controlador.ObtenerTodos();
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
            List<SocioDTO> visibles = new List<SocioDTO>();
            foreach (SocioDTO socio in socios)
            {
                if (Busqueda.Coincide(txtBuscar.Text, socio.Apellido, socio.Nombre, socio.Dni, socio.Telefono, socio.Email,
                    socio.Responsable, socio.Categoria.Texto(), socio.Estado))
                {
                    visibles.Add(socio);
                }
            }

            dgvSocios.MensajeSinFilas(socios.Count == 0
                ? "Todavía no hay socios. Complete el formulario y toque Guardar."
                : "Ningún socio coincide con la búsqueda.");
            dgvSocios.DataSource = null;
            dgvSocios.DataSource = visibles;
            dgvSocios.MostrarColumnas(("Apellido", "Apellido"), ("Nombre", "Nombre"), ("Dni", "DNI"),
                ("Telefono", "Teléfono"), ("Email", "Email"), ("Categoria", "Categoría"), ("Responsable", "Responsable"),
                ("Estado", "Estado"));
            dgvSocios.Columna("Responsable").FillWeight = 160;
            dgvSocios.Columna("Email").FillWeight = 150;
            dgvSocios.MostrarEnumsConTexto();
            dgvSocios.QuitarSeleccion();
        }

        private void dgvSocios_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvSocios.Rows)
            {
                if (fila.DataBoundItem is SocioDTO socio && !socio.Activo)
                {
                    Tema.AtenuarFila(fila);
                }
            }
        }

        private void dgvSocios_SelectionChanged(object? sender, EventArgs e)
        {
            SocioDTO? seleccionado = ObtenerSeleccionado();
            btnEstado.Text = seleccionado != null && !seleccionado.Activo ? "Reactivar" : "Dar de baja";
        }

        private SocioDTO? ObtenerSeleccionado()
        {
            return dgvSocios.Seleccionado<SocioDTO>();
        }

        private void cmbCategoria_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Un Responsable no puede tener responsable.
            bool esResponsable = cmbCategoria.SelectedItem is Categoria.Responsable;
            cmbResponsable.Enabled = !esResponsable;
            if (esResponsable && cmbResponsable.Items.Count > 0)
            {
                cmbResponsable.SelectedIndex = 0;
            }
        }

        // Responsables que se pueden elegir (más "sin responsable"). Al editar, el propio socio no aparece.
        private void CargarResponsables(int idAExcluir, int? seleccionarId)
        {
            cmbResponsable.Items.Clear();
            cmbResponsable.Items.Add(new Opcion<int?>("(Sin responsable)", null));
            try
            {
                foreach (SocioDTO responsable in controlador.ObtenerResponsables())
                {
                    if (responsable.Id != idAExcluir)
                    {
                        cmbResponsable.Items.Add(new Opcion<int?>(responsable.Descripcion, responsable.Id));
                    }
                }
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }

            cmbResponsable.SelectedIndex = 0;
            for (int i = 0; i < cmbResponsable.Items.Count; i++)
            {
                if (cmbResponsable.Items[i] is Opcion<int?> opcion && opcion.Valor == seleccionarId)
                {
                    cmbResponsable.SelectedIndex = i;
                }
            }
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            SocioDTO? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un socio de la grilla para editar.");
                return;
            }
            if (!Formulario.ConfirmarDescartarCambios(HayCambiosSinGuardar()))
            {
                return;
            }

            try
            {
                SocioDTO socio = controlador.ObtenerPorId(seleccionado.Id);
                idSeleccionado = socio.Id;
                CargarFormulario(socio);
                MostrarModoEdicion(socio.Apellido + ", " + socio.Nombre);
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                CargarGrilla();
            }
        }

        private void btnGuardar_Click(object? sender, EventArgs e)
        {
            try
            {
                SocioDTO dto = ArmarDto();
                bool eraEdicion = idSeleccionado != 0;
                controlador.Guardar(dto);

                CargarGrilla();
                LimpiarFormulario();
                dgvSocios.SeleccionarFila("Dni", dto.Dni.Trim());
                Aviso.Mostrar(lblAviso, eraEdicion ? "Socio actualizado." : "Socio guardado.");
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
            SocioDTO? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un socio de la grilla para darlo de baja o reactivarlo.");
                return;
            }

            bool reactivar = !seleccionado.Activo;
            string pregunta = reactivar
                ? "¿Reactivar a " + seleccionado.Descripcion + "?"
                : "¿Dar de baja a " + seleccionado.Descripcion + "? Se conservan sus cuotas y pagos.";
            if (!Mensajes.Confirmar(pregunta))
            {
                return;
            }

            try
            {
                controlador.CambiarEstado(seleccionado.Id, reactivar);
                CargarGrilla();
                if (idSeleccionado == 0)
                {
                    // Un socio dado de baja deja de estar disponible como responsable.
                    LimpiarFormulario();
                }
                dgvSocios.SeleccionarFila("Dni", seleccionado.Dni);
                Aviso.Mostrar(lblAviso, reactivar ? "Socio reactivado." : "Socio dado de baja.");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private void dgvSocios_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnVerFicha_Click(sender, e);
            }
        }

        private void btnVerFicha_Click(object? sender, EventArgs e)
        {
            SocioDTO? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un socio de la grilla para ver su ficha.");
                return;
            }
            Navegacion.AbrirSubpantalla(this, new FrmFichaSocio(seleccionado.Id));
        }

        private void btnExportar_Click(object? sender, EventArgs e)
        {
            ExportarCsv.Grilla(dgvSocios, "Socios");
        }

        private SocioDTO ArmarDto()
        {
            SocioDTO dto = new SocioDTO();
            dto.Id = idSeleccionado;
            dto.Nombre = txtNombre.Text;
            dto.Apellido = txtApellido.Text;
            dto.Dni = txtDni.Text;
            dto.Telefono = txtTelefono.Text;
            dto.Email = txtEmail.Text;
            dto.Categoria = cmbCategoria.SelectedItem is Categoria categoria ? categoria : Categoria.Socio;
            dto.ResponsableId = (cmbResponsable.SelectedItem as Opcion<int?>)?.Valor;
            return dto;
        }

        private void CargarFormulario(SocioDTO dto)
        {
            txtNombre.Text = dto.Nombre;
            txtApellido.Text = dto.Apellido;
            txtDni.Text = dto.Dni;
            txtTelefono.Text = dto.Telefono;
            txtEmail.Text = dto.Email;
            CargarResponsables(dto.Id, dto.ResponsableId);
            cmbCategoria.SelectedItem = dto.Categoria;
        }

        private void MostrarModoEdicion(string nombre)
        {
            lblModo.Text = "Editando: " + nombre;
            tlpBotones.SetColumnSpan(btnGuardar, 1);
            btnCancelarEdicion.Visible = true;
            valoresGuardados = Formulario.Valores(tlpFormulario);
        }

        private void LimpiarFormulario()
        {
            idSeleccionado = 0;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtDni.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            CargarResponsables(0, null);
            cmbCategoria.SelectedIndex = 0;

            lblModo.Text = "Nuevo socio";
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
