using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Ingresos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>
    /// Carga de ingresos que no son cuotas (donaciones, subsidios), a cargo del Operador.
    /// Las cuotas sociales se cobran desde la ficha del socio.
    /// </summary>
    public class IngresoView : UserControl, IRefrescable
    {
        private readonly IngresoController _ingresos;
        private readonly CajaController _cajas;
        private readonly CatalogoController _catalogos;

        private readonly CajaEstadoLabel _estado = new();
        private readonly NumericUpDown _numMonto = new();
        private readonly ComboBox _cmbCategoria = new();
        private readonly ComboBox _cmbMedio = new();
        private readonly Button _btnRegistrar = new();

        public IngresoView() : this(new IngresoController(), new CajaController(), new CatalogoController()) { }

        public IngresoView(IngresoController ingresos, CajaController cajas, CatalogoController catalogos)
        {
            _ingresos = ingresos;
            _cajas = cajas;
            _catalogos = catalogos;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            _numMonto.Location = new Point(20, 30);
            _numMonto.Size = new Size(380, 27);
            _numMonto.DecimalPlaces = 2;
            _numMonto.Maximum = 100_000_000m;
            _numMonto.ThousandsSeparator = true;

            ConfigurarDesplegable(_cmbCategoria, new Point(20, 85));
            _cmbCategoria.DataSource = Enum.GetValues<CategoriaIngreso>()
                .Where(c => c != CategoriaIngreso.CuotaSocial)
                .ToList();

            ConfigurarDesplegable(_cmbMedio, new Point(20, 140));

            _btnRegistrar.Text = "Registrar ingreso";
            _btnRegistrar.Location = new Point(20, 195);
            _btnRegistrar.Size = new Size(380, 38);
            _btnRegistrar.Click += (_, _) => Registrar();

            var formulario = new Panel { Dock = DockStyle.Fill };
            formulario.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Monto", 20, 10), _numMonto,
                Ui.Etiqueta("Categoría", 20, 65), _cmbCategoria,
                Ui.Etiqueta("Medio de pago", 20, 120), _cmbMedio,
                _btnRegistrar
            });

            // Orden de agregado: primero el que llena, después la franja superior.
            Controls.Add(formulario);
            Controls.Add(_estado);
        }

        private static void ConfigurarDesplegable(ComboBox combo, Point ubicacion)
        {
            combo.Location = ubicacion;
            combo.Size = new Size(380, 28);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void Refrescar()
        {
            var abierta = HayCajaAbierta();
            _estado.Mostrar(abierta);
            _btnRegistrar.Enabled = abierta;

            CargarMedios();
        }

        private bool HayCajaAbierta()
        {
            try
            {
                return _cajas.ObtenerActiva() is not null;
            }
            catch (NegocioException)
            {
                return false;
            }
        }

        private void CargarMedios()
        {
            try
            {
                _cmbMedio.DataSource = _catalogos.ObtenerMediosPago().ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void Registrar()
        {
            if (_cmbCategoria.SelectedItem is not CategoriaIngreso categoria || _cmbMedio.SelectedItem is not MedioPagoDto medio)
            {
                Ui.Error("Elegí la categoría y el medio de pago.");
                return;
            }

            var dto = new IngresoCrearDto(_numMonto.Value, categoria, medio.Id);

            try
            {
                _ingresos.Registrar(dto);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            LimpiarFormulario();
            Ui.Info("Ingreso registrado correctamente.");
        }

        private void LimpiarFormulario()
        {
            _numMonto.Value = 0;
            _cmbCategoria.SelectedIndex = 0;
            _cmbMedio.SelectedIndex = 0;
            _numMonto.Focus();
        }
    }
}
