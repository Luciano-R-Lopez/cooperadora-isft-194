using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms.Operador
{
    /// <summary>Pantalla 2.1: inscripción y edición de socios, y listado con buscador.</summary>
    public class SociosView : UserControl
    {
        private readonly SocioController _controller;

        private readonly GroupBox _panelFormulario = new();
        private readonly TextBox _txtNombre = Ui.Campo(15, 43, 255);
        private readonly TextBox _txtApellido = Ui.Campo(15, 98, 255);
        private readonly TextBox _txtDni = Ui.Campo(15, 153, 255);
        private readonly TextBox _txtTelefono = Ui.Campo(15, 208, 255);
        private readonly TextBox _txtEmail = Ui.Campo(15, 263, 255);
        private readonly ComboBox _cmbCategoria = new();
        private readonly ComboBox _cmbResponsable = new();
        private readonly Button _btnLimpiar = new();
        private readonly TextBox _txtBuscar = Ui.Campo(185, 8, 280);
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnVerCuotas = new();
        private readonly Button _btnEditar = new();
        private readonly Button _btnEstado = new();

        /// <summary>Id del socio que se está editando; null cuando el formulario es de alta.</summary>
        private int? _editandoId;

        /// <summary>Se dispara con el id del socio cuyo estado de cuotas se quiere ver.</summary>
        public event Action<int>? VerCuotasSolicitado;

        public SociosView() : this(new SocioController()) { }

        public SociosView(SocioController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            EntrarModoAlta();
            CargarSocios();
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
            _panelFormulario.Width = 290;

            _cmbCategoria.Location = new Point(15, 318);
            _cmbCategoria.Size = new Size(255, 28);
            _cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            Ui.Llenar(_cmbCategoria, Enum.GetValues<Categoria>());

            // Un Responsable no puede tener responsable: se deshabilita y se limpia el desplegable.
            _cmbCategoria.SelectedIndexChanged += (_, _) => ActualizarResponsableHabilitado();

            _cmbResponsable.Location = new Point(15, 373);
            _cmbResponsable.Size = new Size(255, 28);
            _cmbResponsable.DropDownStyle = ComboBoxStyle.DropDownList;

            var btnGuardar = new Button
            {
                Text = "Guardar socio",
                Location = new Point(15, 435),
                Size = new Size(255, 35)
            };
            btnGuardar.Click += (_, _) => GuardarSocio();

            Tema.Secundario(_btnLimpiar);
            _btnLimpiar.Location = new Point(15, 480);
            _btnLimpiar.Size = new Size(255, 32);
            _btnLimpiar.Click += (_, _) => EntrarModoAlta();

            _panelFormulario.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Nombre", 15, 25), _txtNombre,
                Ui.Etiqueta("Apellido", 15, 80), _txtApellido,
                Ui.Etiqueta("DNI", 15, 135), _txtDni,
                Ui.Etiqueta("Teléfono", 15, 190), _txtTelefono,
                Ui.Etiqueta("Email", 15, 245), _txtEmail,
                Ui.Etiqueta("Categoría", 15, 300), _cmbCategoria,
                Ui.Etiqueta("Responsable (opcional)", 15, 355), _cmbResponsable,
                btnGuardar, _btnLimpiar
            });

            return _panelFormulario;
        }

        private Panel ConstruirPanelLista()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0) };

            var barraBusqueda = new Panel { Dock = DockStyle.Top, Height = 40 };
            _txtBuscar.TextChanged += (_, _) => CargarSocios();
            barraBusqueda.Controls.Add(Ui.Etiqueta("Buscar por DNI o nombre:", 5, 12));
            barraBusqueda.Controls.Add(_txtBuscar);

            var barraAcciones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 6, 0, 0) };

            _btnVerCuotas.Text = "Ver estado de cuotas";
            _btnVerCuotas.Size = new Size(180, 32);
            _btnVerCuotas.Click += (_, _) => SolicitarVerCuotas();

            _btnEditar.Text = "Editar socio";
            _btnEditar.Size = new Size(140, 32);
            _btnEditar.Click += (_, _) => EditarSeleccionado();

            _btnEstado.Size = new Size(140, 32);
            _btnEstado.Click += (_, _) => CambiarEstadoSeleccionado();

            var btnExportar = Tema.Secundario(new Button { Text = "Exportar a CSV", Size = new Size(140, 32) });
            btnExportar.Click += (_, _) => ExportarCsv.Grilla(_grilla, "Socios");

            barraAcciones.Controls.AddRange(new Control[] { _btnVerCuotas, _btnEditar, _btnEstado, btnExportar });

            Ui.AgregarColumna(_grilla, "Apellido", nameof(SocioDto.Apellido));
            Ui.AgregarColumna(_grilla, "Nombre", nameof(SocioDto.Nombre));
            Ui.AgregarColumna(_grilla, "DNI", nameof(SocioDto.Dni));
            Ui.AgregarColumna(_grilla, "Teléfono", nameof(SocioDto.Telefono));
            Ui.AgregarColumna(_grilla, "Email", nameof(SocioDto.Email));
            Ui.AgregarColumna(_grilla, "Categoría", nameof(SocioDto.Categoria));
            Ui.AgregarColumna(_grilla, "Responsable", nameof(SocioDto.ResponsableTexto));
            Ui.AgregarColumna(_grilla, "Estado", nameof(SocioDto.EstadoTexto));
            _grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) SolicitarVerCuotas(); };
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();
            _grilla.DataBindingComplete += (_, _) => PintarFilas();

            // Orden de agregado: primero el que llena, después los bordes.
            panel.Controls.Add(_grilla);
            panel.Controls.Add(barraAcciones);
            panel.Controls.Add(barraBusqueda);

            return panel;
        }

        private void GuardarSocio()
        {
            var categoria = (Categoria)_cmbCategoria.SelectedItem!;
            var responsableId = (_cmbResponsable.SelectedItem as Opcion<int?>)?.Valor;

            try
            {
                if (_editandoId is int id)
                {
                    _controller.Editar(new SocioEditarDto(id, _txtNombre.Text, _txtApellido.Text, _txtDni.Text,
                                                          _txtTelefono.Text, _txtEmail.Text, categoria, responsableId));
                }
                else
                {
                    _controller.Registrar(new SocioCrearDto(_txtNombre.Text, _txtApellido.Text, _txtDni.Text,
                                                            _txtTelefono.Text, _txtEmail.Text, categoria, responsableId));
                }
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            var mensaje = _editandoId is null ? "Socio registrado correctamente." : "Socio actualizado correctamente.";
            EntrarModoAlta();
            CargarSocios();
            Ui.Info(mensaje);
        }

        /// <summary>
        /// Recarga los responsables elegibles (más "sin responsable") y elige el indicado.
        /// Al editar, el propio socio no aparece como opción.
        /// </summary>
        private void CargarResponsables(int? excluirId = null, int? seleccionarId = null)
        {
            _cmbResponsable.Items.Clear();
            _cmbResponsable.Items.Add(new Opcion<int?>("(Sin responsable)", null));

            try
            {
                foreach (var r in _controller.ObtenerResponsables().Where(r => r.Id != excluirId))
                    _cmbResponsable.Items.Add(new Opcion<int?>(r.Descripcion, r.Id));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }

            var indice = _cmbResponsable.Items.Cast<Opcion<int?>>().ToList().FindIndex(o => o.Valor == seleccionarId);
            _cmbResponsable.SelectedIndex = Math.Max(0, indice);
        }

        private void ActualizarResponsableHabilitado()
        {
            var esResponsable = _cmbCategoria.SelectedItem is Categoria.Responsable;
            _cmbResponsable.Enabled = !esResponsable;
            if (esResponsable && _cmbResponsable.Items.Count > 0)
                _cmbResponsable.SelectedIndex = 0;
        }

        private void CargarSocios(int? seleccionarId = null)
        {
            try
            {
                _grilla.DataSource = _controller.Buscar(_txtBuscar.Text).ToList();
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
                    if (fila.DataBoundItem is SocioDto s && s.Id == seleccionarId)
                    {
                        fila.Selected = true;
                        _grilla.CurrentCell = fila.Cells[0];
                        break;
                    }
                }
            }

            ActualizarBotones();
        }

        private SocioDto? SocioSeleccionado() => _grilla.CurrentRow?.DataBoundItem as SocioDto;

        private void ActualizarBotones()
        {
            var socio = SocioSeleccionado();
            _btnVerCuotas.Enabled = socio is not null;
            _btnEditar.Enabled = socio is not null;
            _btnEstado.Enabled = socio is not null;
            _btnEstado.Text = socio is { Activo: false } ? "Reactivar" : "Dar de baja";
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is SocioDto { Activo: false })
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
            }
        }

        private void SolicitarVerCuotas()
        {
            if (SocioSeleccionado() is not SocioDto socio)
            {
                Ui.Error("Seleccioná un socio de la lista.");
                return;
            }

            VerCuotasSolicitado?.Invoke(socio.Id);
        }

        private void EditarSeleccionado()
        {
            if (SocioSeleccionado() is not SocioDto socio)
                return;

            _editandoId = socio.Id;
            _panelFormulario.Text = $"Editar socio: {socio.Apellido}, {socio.Nombre}";
            _btnLimpiar.Text = "Cancelar edición";

            _txtNombre.Text = socio.Nombre;
            _txtApellido.Text = socio.Apellido;
            _txtDni.Text = socio.Dni;
            _txtTelefono.Text = socio.Telefono;
            _txtEmail.Text = socio.Email;
            _cmbCategoria.SelectedItem = socio.Categoria;
            CargarResponsables(excluirId: socio.Id, seleccionarId: socio.ResponsableId);
            ActualizarResponsableHabilitado();
            _txtNombre.Focus();
        }

        private void CambiarEstadoSeleccionado()
        {
            if (SocioSeleccionado() is not SocioDto socio)
                return;

            var reactivar = !socio.Activo;
            var pregunta = reactivar
                ? $"¿Reactivar a {socio.NombreCompleto}?"
                : $"¿Dar de baja a {socio.NombreCompleto}? Se conservan sus cuotas y pagos.";
            if (!Ui.Confirmar(pregunta))
                return;

            try
            {
                _controller.CambiarEstado(socio.Id, reactivar);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            // Un socio dado de baja deja de estar disponible como responsable.
            if (_editandoId is null)
                CargarResponsables();
            CargarSocios(socio.Id);
        }

        private void EntrarModoAlta()
        {
            _editandoId = null;
            _panelFormulario.Text = "Nuevo socio";
            _btnLimpiar.Text = "Limpiar formulario";

            _txtNombre.Clear();
            _txtApellido.Clear();
            _txtDni.Clear();
            _txtTelefono.Clear();
            _txtEmail.Clear();
            _cmbCategoria.SelectedIndex = 0;
            CargarResponsables();
            ActualizarResponsableHabilitado();
            _txtNombre.Focus();
        }
    }
}
