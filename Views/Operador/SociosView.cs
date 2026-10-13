using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>Pantalla 2.1: inscripción de socios y listado con buscador.</summary>
    public class SociosView : UserControl
    {
        private readonly SocioController _controller;

        private readonly TextBox _txtNombre = Ui.Campo(15, 43, 255);
        private readonly TextBox _txtApellido = Ui.Campo(15, 98, 255);
        private readonly TextBox _txtDni = Ui.Campo(15, 153, 255);
        private readonly TextBox _txtTelefono = Ui.Campo(15, 208, 255);
        private readonly TextBox _txtEmail = Ui.Campo(15, 263, 255);
        private readonly ComboBox _cmbCategoria = new();
        private readonly ComboBox _cmbResponsable = new();
        private readonly TextBox _txtBuscar = Ui.Campo(165, 8, 280);
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnVerCuotas = new();

        /// <summary>Se dispara con el id del socio cuyo estado de cuotas se quiere ver.</summary>
        public event Action<int>? VerCuotasSolicitado;

        public SociosView() : this(new SocioController()) { }

        public SociosView(SocioController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            CargarResponsables();
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
            var panel = new GroupBox { Text = "Nuevo socio", Dock = DockStyle.Left, Width = 290 };

            _cmbCategoria.Location = new Point(15, 318);
            _cmbCategoria.Size = new Size(255, 28);
            _cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCategoria.DataSource = Enum.GetValues<Categoria>();

            // Un Responsable no puede tener responsable: se deshabilita y se limpia el desplegable.
            _cmbCategoria.SelectedIndexChanged += (_, _) =>
            {
                var esResponsable = _cmbCategoria.SelectedItem is Categoria.Responsable;
                _cmbResponsable.Enabled = !esResponsable;
                if (esResponsable && _cmbResponsable.Items.Count > 0)
                    _cmbResponsable.SelectedIndex = 0;
            };

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

            panel.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Nombre", 15, 25), _txtNombre,
                Ui.Etiqueta("Apellido", 15, 80), _txtApellido,
                Ui.Etiqueta("DNI", 15, 135), _txtDni,
                Ui.Etiqueta("Teléfono", 15, 190), _txtTelefono,
                Ui.Etiqueta("Email", 15, 245), _txtEmail,
                Ui.Etiqueta("Categoría", 15, 300), _cmbCategoria,
                Ui.Etiqueta("Responsable (opcional)", 15, 355), _cmbResponsable,
                btnGuardar
            });

            return panel;
        }

        private Panel ConstruirPanelLista()
        {
            var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0) };

            var barraBusqueda = new Panel { Dock = DockStyle.Top, Height = 40 };
            _txtBuscar.TextChanged += (_, _) => CargarSocios();
            barraBusqueda.Controls.Add(Ui.Etiqueta("Buscar por DNI o nombre:", 5, 12));
            barraBusqueda.Controls.Add(_txtBuscar);
            _txtBuscar.Location = new Point(170, 8);

            var barraAcciones = new Panel { Dock = DockStyle.Bottom, Height = 45 };
            _btnVerCuotas.Text = "Ver estado de cuotas";
            _btnVerCuotas.Size = new Size(180, 32);
            _btnVerCuotas.Location = new Point(5, 8);
            _btnVerCuotas.Click += (_, _) => SolicitarVerCuotas();
            barraAcciones.Controls.Add(_btnVerCuotas);

            Ui.AgregarColumna(_grilla, "Apellido", nameof(SocioDto.Apellido));
            Ui.AgregarColumna(_grilla, "Nombre", nameof(SocioDto.Nombre));
            Ui.AgregarColumna(_grilla, "DNI", nameof(SocioDto.Dni));
            Ui.AgregarColumna(_grilla, "Teléfono", nameof(SocioDto.Telefono));
            Ui.AgregarColumna(_grilla, "Email", nameof(SocioDto.Email));
            Ui.AgregarColumna(_grilla, "Categoría", nameof(SocioDto.Categoria));
            Ui.AgregarColumna(_grilla, "Responsable", nameof(SocioDto.ResponsableTexto));
            _grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) SolicitarVerCuotas(); };

            // Orden de agregado: primero el que llena, después los bordes.
            panel.Controls.Add(_grilla);
            panel.Controls.Add(barraAcciones);
            panel.Controls.Add(barraBusqueda);

            return panel;
        }

        private void GuardarSocio()
        {
            var dto = new SocioCrearDto(
                _txtNombre.Text,
                _txtApellido.Text,
                _txtDni.Text,
                _txtTelefono.Text,
                _txtEmail.Text,
                (Categoria)_cmbCategoria.SelectedItem!,
                (_cmbResponsable.SelectedItem as Opcion<int?>)?.Valor);

            try
            {
                _controller.Registrar(dto);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            LimpiarFormulario();
            CargarResponsables();
            CargarSocios();
            Ui.Info("Socio registrado correctamente.");
        }

        /// <summary>Recarga los responsables elegibles (más "sin responsable") y deja elegida la primera opción.</summary>
        private void CargarResponsables()
        {
            _cmbResponsable.Items.Clear();
            _cmbResponsable.Items.Add(new Opcion<int?>("(Sin responsable)", null));

            try
            {
                foreach (var r in _controller.ObtenerResponsables())
                    _cmbResponsable.Items.Add(new Opcion<int?>(r.Descripcion, r.Id));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }

            _cmbResponsable.SelectedIndex = 0;
        }

        private void CargarSocios()
        {
            try
            {
                _grilla.DataSource = _controller.Buscar(_txtBuscar.Text).ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void SolicitarVerCuotas()
        {
            if (_grilla.CurrentRow?.DataBoundItem is not SocioDto socio)
            {
                Ui.Error("Seleccioná un socio de la lista.");
                return;
            }

            VerCuotasSolicitado?.Invoke(socio.Id);
        }

        private void LimpiarFormulario()
        {
            _txtNombre.Clear();
            _txtApellido.Clear();
            _txtDni.Clear();
            _txtTelefono.Clear();
            _txtEmail.Clear();
            _cmbCategoria.SelectedIndex = 0;
            _txtNombre.Focus();
        }
    }
}
