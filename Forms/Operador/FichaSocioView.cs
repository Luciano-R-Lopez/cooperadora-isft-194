using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms.Operador
{
    /// <summary>Pantalla 2.2: datos del socio y administración de sus cuotas.</summary>
    public class FichaSocioView : UserControl
    {
        private readonly int _socioId;
        private readonly SocioController _socios;
        private readonly CuotaController _cuotas;
        private readonly CatalogoController _catalogos;

        private readonly Label _lblSocio = new();
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnPagar = new();
        private readonly Button _btnGenerar = new();
        private readonly Button _btnPagarACargo = new();

        /// <summary>Datos del socio de la ficha; null si no se pudieron cargar.</summary>
        private SocioDto? _socio;
        private IReadOnlyList<SocioDto> _aCargo = Array.Empty<SocioDto>();

        public event Action? VolverSolicitado;

        public FichaSocioView(int socioId)
            : this(socioId, new SocioController(), new CuotaController(), new CatalogoController()) { }

        public FichaSocioView(int socioId, SocioController socios, CuotaController cuotas, CatalogoController catalogos)
        {
            _socioId = socioId;
            _socios = socios;
            _cuotas = cuotas;
            _catalogos = catalogos;

            ConstruirInterfaz();
            CargarCabecera();
            CargarCuotas();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            _lblSocio.Dock = DockStyle.Top;
            _lblSocio.Height = 90;
            _lblSocio.Padding = new Padding(10, 10, 0, 0);
            _lblSocio.Font = new Font(Font.FontFamily, 11f, FontStyle.Bold);

            Ui.AgregarColumna(_grilla, "Año", nameof(CuotaDto.Anio));
            Ui.AgregarColumna(_grilla, "Monto", nameof(CuotaDto.Monto), "C2");
            Ui.AgregarColumna(_grilla, "Estado", nameof(CuotaDto.Estado));
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();
            _grilla.DataBindingComplete += (_, _) => PintarFilas();

            // Orden de agregado: primero el que llena, después los bordes.
            Controls.Add(_grilla);
            Controls.Add(ConstruirBarraAcciones());
            Controls.Add(_lblSocio);
        }

        private Panel ConstruirBarraAcciones()
        {
            var barra = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(6, 8, 0, 0) };

            var btnVolver = Tema.Secundario(new Button { Text = "← Volver", Size = new Size(100, 32) });
            btnVolver.Click += (_, _) => VolverSolicitado?.Invoke();

            _btnGenerar.Text = "Generar cuota";
            _btnGenerar.Size = new Size(130, 32);
            _btnGenerar.Click += (_, _) => GenerarCuota();

            _btnPagar.Text = "Registrar pago";
            _btnPagar.Size = new Size(130, 32);
            _btnPagar.Click += (_, _) => RegistrarPago();

            _btnPagarACargo.Text = "Pagar cuota de un socio a cargo";
            _btnPagarACargo.Size = new Size(240, 32);
            _btnPagarACargo.Visible = false;
            _btnPagarACargo.Click += (_, _) => PagarCuotaACargo();

            barra.Controls.AddRange(new Control[] { btnVolver, _btnGenerar, _btnPagar, _btnPagarACargo });
            return barra;
        }

        private void CargarCabecera()
        {
            try
            {
                var socio = _socios.ObtenerPorId(_socioId);
                _socio = socio;
                _aCargo = socio.Categoria == Categoria.Responsable ? _socios.ObtenerACargo(socio.Id) : Array.Empty<SocioDto>();

                var baja = socio.Activo ? string.Empty : "   (DADO DE BAJA)";
                _lblSocio.Text = $"{socio.NombreCompleto}{baja}\nDNI: {socio.Dni}   |   Categoría: {socio.Categoria}\n{DescribirVinculo(socio)}";

                // A un socio dado de baja no se le generan cuotas nuevas (sí se le cobran las pendientes).
                _btnGenerar.Enabled = socio.Activo;
                _btnPagarACargo.Visible = _aCargo.Count > 0;
            }
            catch (NegocioException ex)
            {
                _lblSocio.Text = ex.Message;
            }
        }

        /// <summary>Quién es su responsable, o a qué socios tiene a cargo si él es responsable.</summary>
        private string DescribirVinculo(SocioDto socio)
        {
            if (socio.Categoria != Categoria.Responsable)
                return $"Responsable: {socio.ResponsableTexto}";

            return _aCargo.Count == 0
                ? "Socios a cargo: ninguno"
                : "Socios a cargo: " + string.Join("; ", _aCargo.Select(s => s.NombreCompleto));
        }

        private void CargarCuotas(int? seleccionarId = null)
        {
            try
            {
                _grilla.DataSource = _cuotas.ObtenerPorSocio(_socioId).ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            // Se vuelve a seleccionar la cuota con la que se estaba trabajando.
            if (seleccionarId is not null)
            {
                foreach (DataGridViewRow fila in _grilla.Rows)
                {
                    if (fila.DataBoundItem is CuotaDto c && c.Id == seleccionarId)
                    {
                        fila.Selected = true;
                        _grilla.CurrentCell = fila.Cells[0];
                        break;
                    }
                }
            }

            ActualizarBotones();
        }

        private CuotaDto? CuotaSeleccionada() => _grilla.CurrentRow?.DataBoundItem as CuotaDto;

        private void ActualizarBotones()
        {
            var cuota = CuotaSeleccionada();
            _btnPagar.Enabled = cuota is not null && cuota.Estado == EstadoCuota.Pendiente;
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is not CuotaDto cuota) continue;

                fila.DefaultCellStyle.BackColor = cuota.Estado == EstadoCuota.Pagada
                    ? Tema.ExitoSuave
                    : Tema.Superficie;
            }
        }

        private void GenerarCuota()
        {
            using var dialogo = new GenerarCuotaDialog(_socioId, MontoSugerido);
            if (dialogo.ShowDialog(this) != DialogResult.OK || dialogo.Datos is null)
                return;

            try
            {
                var creada = _cuotas.Generar(dialogo.Datos);
                CargarCuotas(creada.Id);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }

        /// <summary>La sugerencia es opcional: si no se puede obtener, el diálogo arranca sin monto.</summary>
        private decimal? MontoSugerido(int anio)
        {
            try
            {
                return _cuotas.ObtenerMontoSugerido(anio);
            }
            catch (NegocioException)
            {
                return null;
            }
        }

        private void RegistrarPago()
        {
            var cuota = CuotaSeleccionada();
            if (cuota is null) return;

            if (ObtenerMedios() is not { } medios)
                return;

            using var dialogo = new RegistrarPagoDialog(cuota, medios, _socio?.Responsable);
            if (dialogo.ShowDialog(this) != DialogResult.OK || dialogo.Medio is not MedioPagoDto medio)
                return;

            var pagadorId = dialogo.PagaResponsable ? _socio?.ResponsableId : null;
            if (Cobrar(cuota.Id, medio.Id, pagadorId))
                CargarCuotas(cuota.Id);
        }

        /// <summary>El responsable elige cuál de sus socios a cargo paga (y qué cuota) desde su propia ficha.</summary>
        private void PagarCuotaACargo()
        {
            if (_socio is null || _aCargo.Count == 0 || ObtenerMedios() is not { } medios)
                return;

            using var dialogo = new PagoACargoDialog(_socio.Descripcion, _aCargo, CuotasDe, medios);
            if (dialogo.ShowDialog(this) != DialogResult.OK
                || dialogo.Cuota is not CuotaDto cuota || dialogo.Medio is not MedioPagoDto medio)
                return;

            Cobrar(cuota.Id, medio.Id, _socio.Id);
        }

        private IReadOnlyList<CuotaDto> CuotasDe(int socioId)
        {
            try
            {
                return _cuotas.ObtenerPorSocio(socioId);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return Array.Empty<CuotaDto>();
            }
        }

        /// <summary>Medios de pago activos; null (con el error ya mostrado) si no hay o no se pudieron obtener.</summary>
        private IReadOnlyList<MedioPagoDto>? ObtenerMedios()
        {
            try
            {
                var medios = _catalogos.ObtenerMediosPago();
                if (medios.Count > 0)
                    return medios;

                Ui.Error("No hay medios de pago activos.");
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }

            return null;
        }

        /// <summary>Registra el cobro y ofrece imprimir el recibo. Devuelve false si no se pudo cobrar.</summary>
        private bool Cobrar(int cuotaId, int medioId, int? pagadorId)
        {
            ReciboPagoDto recibo;
            try
            {
                recibo = _cuotas.RegistrarPago(cuotaId, medioId, pagadorId);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return false;
            }

            if (Ui.Confirmar("Pago registrado. Se generó el ingreso correspondiente en la caja.\n\n¿Querés imprimir el recibo?"))
                Reportes.ImprimirRecibo(recibo);
            return true;
        }
    }
}
