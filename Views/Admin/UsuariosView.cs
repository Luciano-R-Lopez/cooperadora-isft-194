using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Admin
{
    /// <summary>Pantalla 1: gestor de usuarios y roles.</summary>
    public class UsuariosView : UserControl
    {
        private readonly UsuarioController _controller;

        private readonly GroupBox _panelFormulario = new();
        private readonly TextBox _txtUsuario = Ui.Campo(15, 43, 285);
        private readonly TextBox _txtNombre = Ui.Campo(15, 96, 285);
        private readonly TextBox _txtApellido = Ui.Campo(15, 149, 285);
        private readonly TextBox _txtPassword = Ui.Campo(15, 202, 285);
        private readonly Label _lblPassword = Ui.Etiqueta("Contraseña", 15, 182);
        private readonly ComboBox _cmbRol = new();
        private readonly CheckBox _chkActivo = new();
        private readonly Button _btnNuevo = new();
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnEditar = new();
        private readonly Button _btnEstado = new();

        /// <summary>Id del usuario que se está editando; null cuando el formulario es de alta.</summary>
        private int? _editandoId;

        public UsuariosView() : this(new UsuarioController()) { }

        public UsuariosView(UsuarioController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            EntrarModoAlta();
            CargarUsuarios();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            // Se agrega primero el panel que llena el espacio y después el lateral.
            Controls.Add(ConstruirPanelLista());
            Controls.Add(ConstruirPanelFormulario());
        }

        private GroupBox ConstruirPanelFormulario()
        {
            _panelFormulario.Dock = DockStyle.Left;
            _panelFormulario.Width = 320;

            _txtPassword.UseSystemPasswordChar = true;

            _panelFormulario.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Nombre de usuario", 15, 25), _txtUsuario,
                Ui.Etiqueta("Nombre", 15, 78), _txtNombre,
                Ui.Etiqueta("Apellido", 15, 131), _txtApellido,
                _lblPassword, _txtPassword,
                Ui.Etiqueta("Rol", 15, 240)
            });

            _cmbRol.Location = new Point(15, 262);
            _cmbRol.Size = new Size(285, 28);
            _cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbRol.Items.AddRange(Enum.GetValues<Rol>().Cast<object>().ToArray());
            _panelFormulario.Controls.Add(_cmbRol);

            var y = 300;

            _chkActivo.Text = "Usuario activo";
            _chkActivo.Location = new Point(15, y + 8);
            _chkActivo.AutoSize = true;
            _panelFormulario.Controls.Add(_chkActivo);

            var btnGuardar = new Button { Text = "Guardar", Location = new Point(15, y + 45), Size = new Size(285, 36) };
            btnGuardar.Click += (_, _) => Guardar();

            Tema.Secundario(_btnNuevo);
            _btnNuevo.Location = new Point(15, y + 90);
            _btnNuevo.Size = new Size(285, 32);
            _btnNuevo.Click += (_, _) => EntrarModoAlta();

            _panelFormulario.Controls.Add(btnGuardar);
            _panelFormulario.Controls.Add(_btnNuevo);

            return _panelFormulario;
        }

        private Panel ConstruirPanelLista()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0) };

            Ui.AgregarColumna(_grilla, "Id", nameof(UsuarioDto.Id));
            Ui.AgregarColumna(_grilla, "Nombre y apellido", nameof(UsuarioDto.NombreCompleto));
            Ui.AgregarColumna(_grilla, "Usuario", nameof(UsuarioDto.NombreUsuario));
            Ui.AgregarColumna(_grilla, "Rol", nameof(UsuarioDto.RolTexto));
            Ui.AgregarColumna(_grilla, "Estado", nameof(UsuarioDto.EstadoTexto));
            _grilla.Columns[0].FillWeight = 20;
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();
            _grilla.DataBindingComplete += (_, _) => PintarFilas();
            _grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditarSeleccionado(); };

            var barra = new Panel { Dock = DockStyle.Bottom, Height = 45 };

            _btnEditar.Text = "Editar usuario";
            _btnEditar.Location = new Point(5, 8);
            _btnEditar.Size = new Size(140, 32);
            _btnEditar.Click += (_, _) => EditarSeleccionado();

            _btnEstado.Text = "Desactivar usuario";
            _btnEstado.Location = new Point(155, 8);
            _btnEstado.Size = new Size(170, 32);
            _btnEstado.Click += (_, _) => CambiarEstadoSeleccionado();

            barra.Controls.Add(_btnEditar);
            barra.Controls.Add(_btnEstado);

            // Orden de agregado: primero el que llena, después la barra inferior.
            panel.Controls.Add(_grilla);
            panel.Controls.Add(barra);

            return panel;
        }

        private UsuarioDto? UsuarioSeleccionado() => _grilla.CurrentRow?.DataBoundItem as UsuarioDto;

        private void CargarUsuarios(int? seleccionarId = null)
        {
            try
            {
                _grilla.DataSource = _controller.ObtenerTodos().ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            if (seleccionarId is not null)
            {
                foreach (DataGridViewRow fila in _grilla.Rows)
                {
                    if (fila.DataBoundItem is UsuarioDto u && u.Id == seleccionarId)
                    {
                        fila.Selected = true;
                        _grilla.CurrentCell = fila.Cells[0];
                        break;
                    }
                }
            }

            ActualizarBotones();
        }

        private void ActualizarBotones()
        {
            var usuario = UsuarioSeleccionado();
            _btnEditar.Enabled = usuario is not null;
            _btnEstado.Enabled = usuario is not null;
            _btnEstado.Text = usuario is { Activo: false } ? "Activar usuario" : "Desactivar usuario";
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is UsuarioDto { Activo: false })
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
            }
        }

        private void EntrarModoAlta()
        {
            _editandoId = null;

            _panelFormulario.Text = "Nuevo usuario";
            _lblPassword.Text = "Contraseña";
            _btnNuevo.Text = "Limpiar formulario";

            _txtUsuario.Enabled = true;
            _txtUsuario.Clear();
            _txtNombre.Clear();
            _txtApellido.Clear();
            _txtPassword.Clear();
            _chkActivo.Checked = true;
            _cmbRol.SelectedIndex = 0;

            _txtUsuario.Focus();
        }

        private void EntrarModoEdicion(UsuarioDto usuario)
        {
            _editandoId = usuario.Id;

            _panelFormulario.Text = $"Editar usuario: {usuario.NombreUsuario}";
            _lblPassword.Text = "Nueva contraseña (vacía = no cambia)";
            _btnNuevo.Text = "Cancelar edición";

            _txtUsuario.Text = usuario.NombreUsuario;
            _txtUsuario.Enabled = false;
            _txtNombre.Text = usuario.Nombre;
            _txtApellido.Text = usuario.Apellido;
            _txtPassword.Clear();
            _chkActivo.Checked = usuario.Activo;
            _cmbRol.SelectedItem = usuario.Rol;

            _txtNombre.Focus();
        }

        private void EditarSeleccionado()
        {
            if (UsuarioSeleccionado() is not { } usuario)
            {
                Ui.Error("Seleccioná un usuario de la lista.");
                return;
            }

            EntrarModoEdicion(usuario);
        }

        private void Guardar()
        {
            var rol = (Rol)_cmbRol.SelectedItem!;

            UsuarioDto guardado;
            string mensaje;
            try
            {
                if (_editandoId is int id)
                {
                    guardado = _controller.Editar(new UsuarioEditarDto(
                        id, _txtNombre.Text, _txtApellido.Text, _txtPassword.Text, _chkActivo.Checked, rol));
                    mensaje = "Usuario actualizado correctamente.";
                }
                else
                {
                    guardado = _controller.Registrar(new UsuarioCrearDto(
                        _txtUsuario.Text, _txtNombre.Text, _txtApellido.Text, _txtPassword.Text, _chkActivo.Checked, rol));
                    mensaje = "Usuario creado correctamente.";
                }
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            EntrarModoAlta();
            CargarUsuarios(guardado.Id);
            Ui.Info(mensaje);
        }

        private void CambiarEstadoSeleccionado()
        {
            if (UsuarioSeleccionado() is not { } usuario)
            {
                Ui.Error("Seleccioná un usuario de la lista.");
                return;
            }

            var nuevoEstado = !usuario.Activo;
            var accion = nuevoEstado ? "activar" : "desactivar";
            if (!Ui.Confirmar($"¿Querés {accion} al usuario {usuario.NombreUsuario}?"))
                return;

            try
            {
                _controller.CambiarEstado(usuario.Id, nuevoEstado);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            // Si justo se estaba editando a ese usuario, se descarta la edición para no mostrar datos viejos.
            if (_editandoId == usuario.Id)
                EntrarModoAlta();

            CargarUsuarios(usuario.Id);
        }
    }
}
