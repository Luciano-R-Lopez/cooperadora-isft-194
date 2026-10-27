using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Alta y edición de usuarios. Un usuario puede tener varios roles; desactivarlo es su baja.
    public partial class FrmUsuarios : Form
    {
        private readonly UsuarioController controlador;
        private List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
        private int idSeleccionado;
        private string valoresGuardados = string.Empty;

        public FrmUsuarios()
        {
            InitializeComponent();
            Tema.Aplicar(this);
            controlador = new UsuarioController();
            foreach (Rol rol in Enum.GetValues<Rol>())
            {
                lstRoles.Items.Add(rol);
            }
        }

        private void FrmUsuarios_Load(object? sender, EventArgs e)
        {
            CargarGrilla();
            LimpiarFormulario();
        }

        private void FrmUsuarios_FormClosing(object? sender, FormClosingEventArgs e)
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

        private void CargarGrilla()
        {
            try
            {
                usuarios = controlador.ObtenerTodos();
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
            List<UsuarioDTO> visibles = new List<UsuarioDTO>();
            foreach (UsuarioDTO usuario in usuarios)
            {
                if (Busqueda.Coincide(txtBuscar.Text, usuario.NombreCompleto, usuario.NombreUsuario, usuario.RolesTexto, usuario.Estado))
                {
                    visibles.Add(usuario);
                }
            }

            dgvUsuarios.MensajeSinFilas(usuarios.Count == 0
                ? "Todavía no hay usuarios."
                : "Ningún usuario coincide con la búsqueda.");
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = visibles;
            dgvUsuarios.MostrarColumnas(("NombreCompleto", "Nombre y apellido"), ("NombreUsuario", "Usuario"),
                ("RolesTexto", "Roles"), ("Estado", "Estado"));
            dgvUsuarios.QuitarSeleccion();
        }

        private void dgvUsuarios_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvUsuarios.Rows)
            {
                if (fila.DataBoundItem is UsuarioDTO usuario && !usuario.Activo)
                {
                    Tema.AtenuarFila(fila);
                }
            }
        }

        private void dgvUsuarios_SelectionChanged(object? sender, EventArgs e)
        {
            UsuarioDTO? seleccionado = ObtenerSeleccionado();
            btnEstado.Text = seleccionado != null && !seleccionado.Activo ? "Activar" : "Desactivar";
        }

        private UsuarioDTO? ObtenerSeleccionado()
        {
            return dgvUsuarios.Seleccionado<UsuarioDTO>();
        }

        private void dgvUsuarios_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditar_Click(sender, e);
            }
        }

        private void btnEditar_Click(object? sender, EventArgs e)
        {
            UsuarioDTO? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un usuario de la grilla para editar.");
                return;
            }
            if (!Formulario.ConfirmarDescartarCambios(HayCambiosSinGuardar()))
            {
                return;
            }

            try
            {
                UsuarioDTO usuario = controlador.ObtenerPorId(seleccionado.Id);
                CargarFormulario(usuario);
                idSeleccionado = usuario.Id;
                MostrarModoEdicion(usuario.NombreUsuario);
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
                UsuarioDTO dto = ArmarDto();
                bool eraEdicion = idSeleccionado != 0;
                UsuarioDTO guardado = controlador.Guardar(dto);

                CargarGrilla();
                LimpiarFormulario();
                dgvUsuarios.SeleccionarFila("NombreUsuario", guardado.NombreUsuario);
                Aviso.Mostrar(lblAviso, eraEdicion ? "Usuario actualizado." : "Usuario guardado.");
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
            UsuarioDTO? seleccionado = ObtenerSeleccionado();
            if (seleccionado == null)
            {
                Mensajes.Advertencia("Seleccione un usuario de la grilla para activarlo o desactivarlo.");
                return;
            }

            bool activar = !seleccionado.Activo;
            string pregunta = activar
                ? "¿Activar al usuario " + seleccionado.NombreUsuario + "?"
                : "¿Desactivar al usuario " + seleccionado.NombreUsuario + "? No va a poder ingresar a la aplicación.";
            if (!Mensajes.Confirmar(pregunta))
            {
                return;
            }

            try
            {
                controlador.CambiarEstado(seleccionado.Id, activar);
                CargarGrilla();
                if (seleccionado.Id == idSeleccionado)
                {
                    LimpiarFormulario();
                }
                dgvUsuarios.SeleccionarFila("NombreUsuario", seleccionado.NombreUsuario);
                Aviso.Mostrar(lblAviso, activar ? "Usuario activado." : "Usuario desactivado.");
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
            }
        }

        private UsuarioDTO ArmarDto()
        {
            UsuarioDTO dto = new UsuarioDTO();
            dto.Id = idSeleccionado;
            dto.NombreUsuario = txtNombreUsuario.Text;
            dto.Nombre = txtNombre.Text;
            dto.Apellido = txtApellido.Text;
            dto.Password = txtPassword.Text;
            dto.Activo = chkActivo.Checked;
            dto.Roles = lstRoles.CheckedItems.Cast<Rol>().ToList();
            return dto;
        }

        private void CargarFormulario(UsuarioDTO dto)
        {
            txtNombreUsuario.Text = dto.NombreUsuario;
            txtNombre.Text = dto.Nombre;
            txtApellido.Text = dto.Apellido;
            txtPassword.Text = string.Empty;
            chkActivo.Checked = dto.Activo;
            MarcarRoles(dto.Roles);
        }

        private void MarcarRoles(List<Rol> roles)
        {
            for (int i = 0; i < lstRoles.Items.Count; i++)
            {
                lstRoles.SetItemChecked(i, roles.Contains((Rol)lstRoles.Items[i]));
            }
        }

        private void MostrarModoEdicion(string nombreUsuario)
        {
            lblModo.Text = "Editando: " + nombreUsuario;
            lblPassword.Text = "Nueva contraseña (vacía = no cambia):";
            txtNombreUsuario.Enabled = false;
            tlpBotones.SetColumnSpan(btnGuardar, 1);
            btnCancelarEdicion.Visible = true;
            valoresGuardados = Formulario.Valores(tlpFormulario);
        }

        private void LimpiarFormulario()
        {
            idSeleccionado = 0;
            txtNombreUsuario.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtPassword.Text = string.Empty;
            chkActivo.Checked = true;
            MarcarRoles(new List<Rol> { Rol.Operador });

            lblModo.Text = "Nuevo usuario";
            lblPassword.Text = "Contraseña: *";
            txtNombreUsuario.Enabled = true;
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
