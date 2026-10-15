using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Pantalla 3.1: apertura, control y cierre de caja.</summary>
    public class CajaView : UserControl, IRefrescable
    {
        private readonly CajaController _controller;

        private readonly CajaEstadoLabel _estado = new();
        private readonly Label _lblSaldo = new();
        private readonly NumericUpDown _numSaldo = new();
        private readonly Button _btnAccion = new();

        private readonly Label _lblSaldoInicial = new();
        private readonly Label _lblIngresos = new();
        private readonly Label _lblEgresos = new();
        private readonly Label _lblEsperado = new();
        private readonly Label _lblDiferencia = new();
        private readonly DataGridView _grillaMedios = Ui.CrearGrilla();

        private bool _cajaAbierta;
        private bool _saldoIngresadoManualmente;
        private bool _actualizando;

        public CajaView() : this(new CajaController()) { }

        public CajaView(CajaController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            var contenido = new Panel { Size = new Size(760, 410) };
            contenido.Controls.Add(ConstruirPanelAccion());
            contenido.Controls.Add(ConstruirPanelResumen());
            contenido.Controls.Add(ConstruirPanelMedios());

            // Orden de agregado: primero el que llena, después la franja superior.
            Controls.Add(Ui.Centrado(contenido, margenSuperior: 10));
            Controls.Add(_estado);
        }

        private GroupBox ConstruirPanelAccion()
        {
            var panel = new GroupBox { Text = "Apertura / Cierre", Location = new Point(20, 20), Size = new Size(340, 160) };

            _lblSaldo.Location = new Point(15, 30);
            _lblSaldo.AutoSize = true;

            ConfigurarCampoMonto(_numSaldo, new Point(15, 55), 300);
            _numSaldo.ValueChanged += (_, _) =>
            {
                if (_actualizando) return;
                _saldoIngresadoManualmente = true;
                ActualizarResumen();
            };

            _btnAccion.Location = new Point(15, 105);
            _btnAccion.Size = new Size(300, 38);
            _btnAccion.Click += (_, _) => AbrirOCerrar();

            panel.Controls.AddRange(new Control[] { _lblSaldo, _numSaldo, _btnAccion });
            return panel;
        }

        private GroupBox ConstruirPanelResumen()
        {
            var panel = new GroupBox { Text = "Resumen de caja", Location = new Point(380, 20), Size = new Size(360, 200) };

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(10, 5, 10, 5)
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));

            AgregarFila(tabla, 0, "Saldo inicial", _lblSaldoInicial);
            AgregarFila(tabla, 1, "Total ingresos (+)", _lblIngresos);
            AgregarFila(tabla, 2, "Total egresos (-)", _lblEgresos);
            AgregarFila(tabla, 3, "Saldo esperado", _lblEsperado);
            AgregarFila(tabla, 4, "Diferencia hallada", _lblDiferencia);

            _lblEsperado.Font = new Font(Font.FontFamily, 9f, FontStyle.Bold);

            panel.Controls.Add(tabla);
            return panel;
        }

        private GroupBox ConstruirPanelMedios()
        {
            var panel = new GroupBox { Text = "Saldo por medio de pago", Location = new Point(380, 235), Size = new Size(360, 160) };

            Ui.AgregarColumna(_grillaMedios, "Medio de pago", nameof(TotalMedioPagoDto.MedioPago));
            Ui.AgregarColumna(_grillaMedios, "Saldo", nameof(TotalMedioPagoDto.Monto), "C2");

            panel.Controls.Add(_grillaMedios);
            return panel;
        }

        private static void AgregarFila(TableLayoutPanel tabla, int fila, string titulo, Label valor)
        {
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            tabla.Controls.Add(new Label { Text = titulo, AutoSize = true, Anchor = AnchorStyles.Left }, 0, fila);

            valor.AutoSize = true;
            valor.Anchor = AnchorStyles.Right;
            tabla.Controls.Add(valor, 1, fila);
        }

        private static void ConfigurarCampoMonto(NumericUpDown campo, Point ubicacion, int ancho)
        {
            campo.Location = ubicacion;
            campo.Size = new Size(ancho, 27);
            campo.DecimalPlaces = 2;
            campo.Maximum = 100_000_000m;
            campo.ThousandsSeparator = true;
        }

        public void Refrescar()
        {
            CajaDto? activa;
            try
            {
                activa = _controller.ObtenerActiva();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            _cajaAbierta = activa is not null;
            _saldoIngresadoManualmente = false;

            _estado.Mostrar(_cajaAbierta);
            _lblSaldo.Text = _cajaAbierta ? "Saldo declarado (dinero contado al cerrar)" : "Saldo inicial";
            _btnAccion.Text = _cajaAbierta ? "Cerrar caja" : "Abrir caja";

            _actualizando = true;
            _numSaldo.Value = 0;
            _actualizando = false;

            ActualizarResumen();
        }

        private void ActualizarResumen()
        {
            if (!_cajaAbierta)
            {
                MostrarResumenVacio();
                return;
            }

            // Mientras no se tipee el saldo declarado, no se muestra una diferencia engañosa.
            decimal? declarado = _saldoIngresadoManualmente ? _numSaldo.Value : null;

            try
            {
                MostrarResumen(_controller.ObtenerResumenActivo(declarado));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        private void MostrarResumen(ResumenCajaDto resumen)
        {
            _lblSaldoInicial.Text = resumen.MontoInicial.ToString("C2");
            _lblIngresos.Text = resumen.TotalIngresos.ToString("C2");
            _lblEgresos.Text = resumen.TotalEgresos.ToString("C2");
            _lblEsperado.Text = resumen.MontoEsperado.ToString("C2");
            _grillaMedios.DataSource = resumen.TotalesPorMedio.ToList();

            if (resumen.Diferencia is decimal diferencia)
            {
                _lblDiferencia.Text = diferencia.ToString("C2");
                _lblDiferencia.ForeColor = diferencia == 0 ? Tema.Texto : Tema.PeligroTexto;
            }
            else
            {
                _lblDiferencia.Text = "-";
                _lblDiferencia.ForeColor = Tema.Texto;
            }
        }

        private void MostrarResumenVacio()
        {
            foreach (var etiqueta in new[] { _lblSaldoInicial, _lblIngresos, _lblEgresos, _lblEsperado, _lblDiferencia })
            {
                etiqueta.Text = "-";
                etiqueta.ForeColor = Tema.Texto;
            }

            _grillaMedios.DataSource = new List<TotalMedioPagoDto>();
        }

        private void AbrirOCerrar()
        {
            if (_cajaAbierta)
                CerrarCaja();
            else
                AbrirCaja();
        }

        private void AbrirCaja()
        {
            try
            {
                _controller.Abrir(new AbrirCajaDto(_numSaldo.Value));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Refrescar();
        }

        private void CerrarCaja()
        {
            if (!Ui.Confirmar($"¿Cerrar la caja con un saldo declarado de {_numSaldo.Value:C2}?"))
                return;

            ResumenCajaDto resumen;
            try
            {
                resumen = _controller.Cerrar(new CerrarCajaDto(_numSaldo.Value));
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Ui.Info(
                $"Caja cerrada.\n\n" +
                $"Saldo esperado: {resumen.MontoEsperado:C2}\n" +
                $"Saldo declarado: {resumen.MontoDeclarado:C2}\n" +
                $"Diferencia: {resumen.Diferencia:C2}");

            Refrescar();
        }
    }
}
