using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>
    /// Cuotas del año (valor y generación para todos los socios activos) y listado de morosos:
    /// socios con cuotas pendientes, exportable a CSV.
    /// </summary>
    public class CuotasView : UserControl, IRefrescable
    {
        private readonly CuotaController _cuotas;

        private readonly NumericUpDown _numAnio = new() { Minimum = 2000, Maximum = 2100, Width = 90 };
        private readonly NumericUpDown _numValor = new() { DecimalPlaces = 2, Maximum = 100_000_000m, ThousandsSeparator = true, Width = 160 };
        private readonly ComboBox _cmbAnioMorosos = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 120 };
        private readonly Label _lblTotal = new() { AutoSize = true, Margin = new Padding(20, 14, 0, 0) };
        private readonly DataGridView _grilla = Ui.CrearGrilla();

        private bool _cargando;

        public CuotasView() : this(new CuotaController()) { }

        public CuotasView(CuotaController cuotas)
        {
            _cuotas = cuotas;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            // Valor de la cuota y generación del año.
            var grupoAnio = new GroupBox { Text = "Cuota del año", Dock = DockStyle.Top, Height = 95, Padding = new Padding(10) };
            var filaAnio = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };

            _numAnio.Value = DateTime.Today.Year;
            _numAnio.ValueChanged += (_, _) => ProponerValor();

            var btnGuardarValor = Tema.Secundario(new Button { Text = "Guardar valor", Size = new Size(130, 32) });
            btnGuardarValor.Click += (_, _) => GuardarValor();

            var btnGenerar = new Button { Text = "Generar cuotas para todos los socios activos", Size = new Size(330, 32) };
            btnGenerar.Click += (_, _) => GenerarParaTodos();

            filaAnio.Controls.AddRange(new Control[]
            {
                Etiqueta("Año"), _numAnio, Etiqueta("Valor de la cuota"), _numValor, btnGuardarValor, btnGenerar
            });
            grupoAnio.Controls.Add(filaAnio);

            // Morosos.
            var filtros = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(0, 10, 0, 0), WrapContents = false };
            _cmbAnioMorosos.SelectedIndexChanged += (_, _) => CargarMorosos();
            var btnExportar = Tema.Secundario(new Button { Text = "Exportar a CSV", Size = new Size(140, 32) });
            btnExportar.Click += (_, _) => ExportarCsv.Grilla(_grilla, "Morosos");
            filtros.Controls.AddRange(new Control[]
            {
                new Label { Text = "Morosos — cuotas pendientes del año", AutoSize = true, Margin = new Padding(3, 8, 3, 0),
                            Font = new Font(Font, FontStyle.Bold) },
                _cmbAnioMorosos, btnExportar, _lblTotal
            });

            Ui.AgregarColumna(_grilla, "Año", nameof(CuotaPendienteDto.Anio));
            Ui.AgregarColumna(_grilla, "Apellido", nameof(CuotaPendienteDto.Apellido));
            Ui.AgregarColumna(_grilla, "Nombre", nameof(CuotaPendienteDto.Nombre));
            Ui.AgregarColumna(_grilla, "DNI", nameof(CuotaPendienteDto.Dni));
            Ui.AgregarColumna(_grilla, "Teléfono", nameof(CuotaPendienteDto.Telefono));
            Ui.AgregarColumna(_grilla, "Email", nameof(CuotaPendienteDto.Email));
            Ui.AgregarColumna(_grilla, "Responsable", nameof(CuotaPendienteDto.Responsable));
            Ui.AgregarColumna(_grilla, "Monto", nameof(CuotaPendienteDto.Monto), "C2");
            _grilla.Columns[0].FillWeight = 40;
            _grilla.DataBindingComplete += (_, _) => PintarFilas();

            // Orden de agregado: primero el que llena, después los bordes (de abajo hacia arriba).
            Controls.Add(_grilla);
            Controls.Add(filtros);
            Controls.Add(grupoAnio);
        }

        private static Label Etiqueta(string texto) =>
            new() { Text = texto, AutoSize = true, Margin = new Padding(10, 8, 3, 0) };

        public void Refrescar()
        {
            ProponerValor();

            // Años con cuotas pendientes, más "Todos"; se mantiene el elegido si sigue en la lista.
            var elegido = (_cmbAnioMorosos.SelectedItem as Opcion<int?>)?.Valor;
            IReadOnlyList<CuotaPendienteDto> pendientes;
            try
            {
                pendientes = _cuotas.ObtenerPendientes(null);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            var opciones = new List<Opcion<int?>> { new("Todos", null) };
            opciones.AddRange(pendientes.Select(p => p.Anio).Distinct().OrderByDescending(a => a)
                                        .Select(a => new Opcion<int?>(a.ToString(), a)));

            _cargando = true;
            Ui.Llenar(_cmbAnioMorosos, opciones, opciones.FindIndex(o => o.Valor == elegido));
            _cargando = false;

            CargarMorosos();
        }

        private void CargarMorosos()
        {
            if (_cargando) return;

            var anio = (_cmbAnioMorosos.SelectedItem as Opcion<int?>)?.Valor;
            try
            {
                var pendientes = _cuotas.ObtenerPendientes(anio);
                _grilla.DataSource = pendientes.ToList();
                _lblTotal.Text = $"{pendientes.Count} cuotas pendientes — total adeudado {pendientes.Sum(p => p.Monto):C2}";
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is CuotaPendienteDto { SocioActivo: false })
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
            }
        }

        /// <summary>Muestra el valor fijado para el año elegido (o el de la última cuota de ese año).</summary>
        private void ProponerValor()
        {
            try
            {
                var sugerido = _cuotas.ObtenerMontoSugerido((int)_numAnio.Value) ?? 0m;
                _numValor.Value = Math.Min(sugerido, _numValor.Maximum);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void GuardarValor()
        {
            try
            {
                _cuotas.FijarValorCuota((int)_numAnio.Value, _numValor.Value);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Ui.Info($"Valor de la cuota {_numAnio.Value} guardado: {_numValor.Value:C2}.");
        }

        private void GenerarParaTodos()
        {
            var anio = (int)_numAnio.Value;
            if (!Ui.Confirmar($"¿Generar la cuota {anio} de {_numValor.Value:C2} a todos los socios activos que todavía no la tienen?"))
                return;

            GeneracionCuotasDto resultado;
            try
            {
                resultado = _cuotas.GenerarParaTodos(anio, _numValor.Value);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Refrescar();
            Ui.Info($"Se generaron {resultado.Generadas} cuotas {resultado.Anio} de {resultado.Monto:C2}.\n" +
                    $"{resultado.YaTenian} socios ya tenían la cuota de ese año.");
        }
    }
}
