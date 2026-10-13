using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
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
            var barra = new Panel { Dock = DockStyle.Bottom, Height = 50 };

            var btnVolver = new Button { Text = "← Volver", Location = new Point(10, 9), Size = new Size(100, 32) };
            btnVolver.Click += (_, _) => VolverSolicitado?.Invoke();

            var btnGenerar = new Button { Text = "Generar cuota", Location = new Point(130, 9), Size = new Size(130, 32) };
            btnGenerar.Click += (_, _) => GenerarCuota();

            _btnPagar.Text = "Registrar pago";
            _btnPagar.Location = new Point(280, 9);
            _btnPagar.Size = new Size(130, 32);
            _btnPagar.Click += (_, _) => RegistrarPago();

            barra.Controls.AddRange(new Control[] { btnVolver, btnGenerar, _btnPagar });
            return barra;
        }

        private void CargarCabecera()
        {
            try
            {
                var socio = _socios.ObtenerPorId(_socioId);
                _lblSocio.Text = $"{socio.NombreCompleto}\nDNI: {socio.Dni}   |   Categoría: {socio.Categoria}\n{DescribirVinculo(socio)}";
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

            var aCargo = _socios.ObtenerACargo(socio.Id);
            return aCargo.Count == 0
                ? "Socios a cargo: ninguno"
                : "Socios a cargo: " + string.Join("; ", aCargo.Select(s => s.NombreCompleto));
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
                    ? Color.Honeydew
                    : SystemColors.Window;
            }
        }

        private void GenerarCuota()
        {
            using var dialogo = new GenerarCuotaDialog(_socioId);
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

        private void RegistrarPago()
        {
            var cuota = CuotaSeleccionada();
            if (cuota is null) return;

            IReadOnlyList<MedioPagoDto> medios;
            try
            {
                medios = _catalogos.ObtenerMediosPago();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            using var dialogo = new RegistrarPagoDialog(cuota, medios);
            if (dialogo.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                _cuotas.RegistrarPago(cuota.Id, dialogo.Medio.Id);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            CargarCuotas(cuota.Id);
            Ui.Info("Pago registrado. Se generó el ingreso correspondiente en la caja.");
        }
    }
}
