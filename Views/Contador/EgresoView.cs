using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Egresos;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Pantalla 3.3: carga de egresos.</summary>
    public class EgresoView : UserControl, IRefrescable
    {
        private readonly EgresoController _egresos;
        private readonly CajaController _cajas;
        private readonly CatalogoController _catalogos;

        private readonly CajaEstadoLabel _estado = new();
        private readonly NumericUpDown _numMonto = new();
        private readonly ComboBox _cmbMotivo = new();
        private readonly ComboBox _cmbMedio = new();
        private readonly Button _btnRegistrar = new();

        public EgresoView() : this(new EgresoController(), new CajaController(), new CatalogoController()) { }

        public EgresoView(EgresoController egresos, CajaController cajas, CatalogoController catalogos)
        {
            _egresos = egresos;
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

            ConfigurarDesplegable(_cmbMotivo, new Point(20, 85));
            ConfigurarDesplegable(_cmbMedio, new Point(20, 140));

            _btnRegistrar.Text = "Registrar egreso";
            _btnRegistrar.Location = new Point(20, 195);
            _btnRegistrar.Size = new Size(380, 38);
            _btnRegistrar.Click += (_, _) => Registrar();

            var formulario = new Panel { Dock = DockStyle.Fill };
            formulario.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Monto", 20, 10), _numMonto,
                Ui.Etiqueta("Motivo", 20, 65), _cmbMotivo,
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

            CargarOpciones();
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

        private void CargarOpciones()
        {
            try
            {
                _cmbMotivo.DataSource = _catalogos.ObtenerMotivosEgreso().ToList();
                _cmbMedio.DataSource = _catalogos.ObtenerMediosPago().ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void Registrar()
        {
            if (_cmbMotivo.SelectedItem is not MotivoEgresoDto motivo || _cmbMedio.SelectedItem is not MedioPagoDto medio)
            {
                Ui.Error("Elegí el motivo y el medio de pago.");
                return;
            }

            var dto = new EgresoCrearDto(_numMonto.Value, motivo.Id, medio.Id);

            try
            {
                _egresos.Registrar(dto);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            LimpiarFormulario();
            Ui.Info("Egreso registrado correctamente.");
        }

        private void LimpiarFormulario()
        {
            _numMonto.Value = 0;
            _cmbMotivo.SelectedIndex = 0;
            _cmbMedio.SelectedIndex = 0;
            _numMonto.Focus();
        }
    }
}
