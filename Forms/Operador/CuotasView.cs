using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms.Operador
{
    /// <summary>
    /// Cuotas del año (valor y generación para todos los socios activos) y lista de morosos: un socio por fila
    /// con sus cuotas pendientes, desde donde se cobra, se abre la ficha o se copian los emails.
    /// </summary>
    public class CuotasView : UserControl, IRefrescable
    {
        private readonly CuotaController _cuotas;
        private readonly SocioController _socios;
        private readonly CatalogoController _catalogos;

        private readonly NumericUpDown _numAnio = new() { Minimum = 2000, Maximum = 2100, Width = 90 };
        private readonly NumericUpDown _numValor = new() { DecimalPlaces = 2, Maximum = 100_000_000m, ThousandsSeparator = true, Width = 160 };

        private readonly ComboBox _cmbAnioMorosos = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 150 };
        private readonly TextBox _txtBuscar = new() { Width = 220 };
        private readonly CheckBox _chkDadosDeBaja = new() { Text = "Incluir dados de baja", AutoSize = true, Checked = true, Margin = new Padding(15, 8, 3, 0) };
        private readonly Label _lblTotal = new() { AutoSize = true, Margin = new Padding(0, 10, 0, 0) };
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnCobrar = new();
        private readonly Button _btnFicha = new();

        private bool _cargando;

        public CuotasView() : this(new CuotaController(), new SocioController(), new CatalogoController()) { }

        public CuotasView(CuotaController cuotas, SocioController socios, CatalogoController catalogos)
        {
            _cuotas = cuotas;
            _socios = socios;
            _catalogos = catalogos;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            // Orden de agregado: primero el que llena, después los bordes (de abajo hacia arriba).
            Controls.Add(_grilla);
            Controls.Add(ConstruirAcciones());
            Controls.Add(ConstruirFiltros());
            Controls.Add(ConstruirCuotaDelAnio());

            Ui.AgregarColumna(_grilla, "Socio", nameof(DeudorDto.NombreCompleto));
            Ui.AgregarColumna(_grilla, "DNI", nameof(DeudorDto.Dni));
            Ui.AgregarColumna(_grilla, "Paga", nameof(DeudorDto.Paga));
            Ui.AgregarColumna(_grilla, "Años que debe", nameof(DeudorDto.AniosTexto));
            Ui.AgregarColumna(_grilla, "Total", nameof(DeudorDto.Total), "C2");
            Ui.AgregarColumna(_grilla, "Teléfono", nameof(DeudorDto.Telefono));
            Ui.AgregarColumna(_grilla, "Email", nameof(DeudorDto.Email));
            Ui.AgregarColumna(_grilla, "Estado", nameof(DeudorDto.EstadoTexto));
            _grilla.Columns[2].FillWeight = 150;
            _grilla.Columns[7].FillWeight = 60;
            _grilla.DataBindingComplete += (_, _) => PintarFilas();
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();
            _grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) CobrarSeleccionado(); };
        }

        private GroupBox ConstruirCuotaDelAnio()
        {
            var grupo = new GroupBox { Text = "Cuota del año", Dock = DockStyle.Top, Height = 95, Padding = new Padding(10) };
            var fila = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };

            _numAnio.Value = DateTime.Today.Year;
            _numAnio.ValueChanged += (_, _) => ProponerValor();

            var btnGuardarValor = Tema.Secundario(new Button { Text = "Guardar valor", Size = new Size(130, 32) });
            btnGuardarValor.Click += (_, _) => GuardarValor();

            var btnGenerar = new Button { Text = "Generar cuotas para todos los socios activos", Size = new Size(330, 32) };
            btnGenerar.Click += (_, _) => GenerarParaTodos();

            fila.Controls.AddRange(new Control[]
            {
                Etiqueta("Año"), _numAnio, Etiqueta("Valor de la cuota"), _numValor, btnGuardarValor, btnGenerar
            });
            grupo.Controls.Add(fila);
            return grupo;
        }

        private FlowLayoutPanel ConstruirFiltros()
        {
            var filtros = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(0, 10, 0, 0), WrapContents = false };

            _cmbAnioMorosos.SelectedIndexChanged += (_, _) => CargarMorosos();
            _txtBuscar.TextChanged += (_, _) => CargarMorosos();
            _chkDadosDeBaja.CheckedChanged += (_, _) => CargarMorosos();

            filtros.Controls.AddRange(new Control[]
            {
                new Label { Text = "Morosos", AutoSize = true, Margin = new Padding(3, 8, 10, 0), Font = new Font(Font, FontStyle.Bold) },
                Etiqueta("Cuotas de"), _cmbAnioMorosos,
                Etiqueta("Buscar socio o DNI"), _txtBuscar,
                _chkDadosDeBaja
            });
            return filtros;
        }

        private FlowLayoutPanel ConstruirAcciones()
        {
            var acciones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 6, 0, 0), WrapContents = false };

            _btnCobrar.Text = "Cobrar cuotas";
            _btnCobrar.Size = new Size(140, 32);
            _btnCobrar.Click += (_, _) => CobrarSeleccionado();

            _btnFicha.Text = "Ver ficha";
            _btnFicha.Size = new Size(110, 32);
            Tema.Secundario(_btnFicha);
            _btnFicha.Click += (_, _) => VerFicha();

            var btnEmails = Tema.Secundario(new Button { Text = "Copiar emails", Size = new Size(130, 32) });
            btnEmails.Click += (_, _) => CopiarEmails();

            var btnExportar = Tema.Secundario(new Button { Text = "Exportar a CSV", Size = new Size(140, 32) });
            btnExportar.Click += (_, _) => ExportarCsv.Grilla(_grilla, "Morosos");

            _lblTotal.Margin = new Padding(20, 14, 0, 0);
            acciones.Controls.AddRange(new Control[] { _btnCobrar, _btnFicha, btnEmails, btnExportar, _lblTotal });
            return acciones;
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

            var opciones = new List<Opcion<int?>> { new("Todos los años", null) };
            opciones.AddRange(pendientes.Select(p => p.Anio).Distinct().OrderByDescending(a => a)
                                        .Select(a => new Opcion<int?>(a.ToString(), a)));

            _cargando = true;
            Ui.Llenar(_cmbAnioMorosos, opciones, opciones.FindIndex(o => o.Valor == elegido));
            _cargando = false;

            CargarMorosos();
        }

        private void CargarMorosos(int? seleccionarSocioId = null)
        {
            if (_cargando) return;

            var seleccionado = seleccionarSocioId ?? Seleccionado()?.SocioId;
            var filtro = new DeudoresFiltroDto((_cmbAnioMorosos.SelectedItem as Opcion<int?>)?.Valor, _txtBuscar.Text, _chkDadosDeBaja.Checked);

            IReadOnlyList<DeudorDto> deudores;
            try
            {
                deudores = _cuotas.ObtenerDeudores(filtro);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            _grilla.DataSource = deudores.ToList();
            _lblTotal.Text = $"{deudores.Count} socios deben {deudores.Sum(d => d.CantidadCuotas)} cuotas — total {deudores.Sum(d => d.Total):C2}";

            // Se mantiene elegido el socio con el que se estaba trabajando (si sigue debiendo).
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is DeudorDto d && d.SocioId == seleccionado)
                {
                    fila.Selected = true;
                    _grilla.CurrentCell = fila.Cells[0];
                    break;
                }
            }

            ActualizarBotones();
        }

        private DeudorDto? Seleccionado() => _grilla.CurrentRow?.DataBoundItem as DeudorDto;

        private void ActualizarBotones()
        {
            var hay = Seleccionado() is not null;
            _btnCobrar.Enabled = hay;
            _btnFicha.Enabled = hay;
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is DeudorDto { SocioActivo: false })
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
            }
        }

        /// <summary>Cobra en un solo pago las cuotas pendientes que se elijan del socio seleccionado.</summary>
        private void CobrarSeleccionado()
        {
            if (Seleccionado() is not DeudorDto deudor)
                return;

            IReadOnlyList<CuotaDto> pendientes;
            IReadOnlyList<MedioPagoDto> medios;
            try
            {
                pendientes = _cuotas.ObtenerPorSocio(deudor.SocioId)
                                    .Where(c => c.Estado == Common.Enums.EstadoCuota.Pendiente)
                                    .ToList();
                medios = _catalogos.ObtenerMediosPago();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            if (medios.Count == 0)
            {
                Ui.Error("No hay medios de pago activos.");
                return;
            }

            using var dialogo = new CobrarCuotasDialog($"{deudor.NombreCompleto} ({deudor.Dni})", pendientes, medios, deudor.Responsable);
            if (dialogo.ShowDialog(this) != DialogResult.OK || dialogo.Medio is not { } medio || dialogo.Cuotas.Count == 0)
                return;

            ReciboPagoDto recibo;
            try
            {
                recibo = _cuotas.RegistrarPagos(dialogo.Cuotas.Select(c => c.Id).ToList(), medio.Id,
                                                dialogo.PagaResponsable ? deudor.ResponsableId : null);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Refrescar();
            if (Ui.Confirmar($"Se cobraron {recibo.Cuotas.Count} cuota(s) por {recibo.Monto:C2}.\n\n¿Querés imprimir el recibo?"))
                Reportes.ImprimirRecibo(recibo);
        }

        /// <summary>La ficha del socio en una ventana aparte, para no perder la lista.</summary>
        private void VerFicha()
        {
            if (Seleccionado() is not DeudorDto deudor)
                return;

            using (var ventana = new Form
            {
                Text = $"Ficha de {deudor.NombreCompleto}",
                StartPosition = FormStartPosition.CenterParent,
                ClientSize = new Size(1000, 620),
                MinimizeBox = false
            })
            {
                var ficha = new FichaSocioView(deudor.SocioId);
                ficha.VolverSolicitado += ventana.Close;
                ventana.Controls.Add(ficha);
                Tema.Aplicar(ventana);
                ventana.ShowDialog(this);
            }

            // En la ficha se pudo cobrar o generar cuotas.
            Refrescar();
        }

        /// <summary>Emails de los socios de la lista (o de su responsable, que es quien paga), separados por ";".</summary>
        private void CopiarEmails()
        {
            IReadOnlyList<DeudorDto> deudores = _grilla.DataSource as List<DeudorDto> ?? new List<DeudorDto>();

            var emails = new List<string>();
            foreach (var deudor in deudores)
            {
                var email = deudor.Email;
                if (deudor.ResponsableId is int responsableId)
                {
                    try
                    {
                        var responsable = _socios.ObtenerPorId(responsableId);
                        if (responsable.Email.Length > 0)
                            email = responsable.Email;
                    }
                    catch (NegocioException)
                    {
                        // Sin datos del responsable se usa el email del socio.
                    }
                }

                if (email.Length > 0)
                    emails.Add(email);
            }

            var distintos = emails.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (distintos.Count == 0)
            {
                Ui.Info("Ninguno de los socios de la lista tiene email cargado.");
                return;
            }

            Clipboard.SetText(string.Join("; ", distintos));
            var sinEmail = deudores.Count - emails.Count;
            Ui.Info($"Se copiaron {distintos.Count} emails. Pegalos en el campo \"CCO\" de tu correo para enviar un recordatorio." +
                    (sinEmail > 0 ? $"\n\n{sinEmail} socio(s) de la lista no tienen email: habrá que llamarlos." : string.Empty));
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
