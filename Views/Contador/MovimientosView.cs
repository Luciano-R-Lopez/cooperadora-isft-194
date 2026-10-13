using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Cajas;
using CooperadoraISFT94.DTOs.Movimientos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Dashboard: histórico filtrable de ingresos y egresos de una caja.</summary>
    public class MovimientosView : UserControl, IRefrescable
    {
        private readonly MovimientoController _controller;
        private readonly CatalogoController _catalogos;

        private readonly ComboBox _cmbCaja = new();
        private readonly ComboBox _cmbTipo = new();
        private readonly ComboBox _cmbMedio = new();
        private readonly TextBox _txtBuscar = Ui.Campo(740, 18, 200);
        private readonly DataGridView _grilla = Ui.CrearGrilla();

        private bool _cargando;

        public MovimientosView() : this(new MovimientoController(), new CatalogoController()) { }

        public MovimientosView(MovimientoController controller, CatalogoController catalogos)
        {
            _controller = controller;
            _catalogos = catalogos;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            var barra = new Panel { Dock = DockStyle.Top, Height = 55 };

            _cmbCaja.Location = new Point(10, 18);
            _cmbCaja.Size = new Size(280, 28);
            _cmbCaja.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCaja.DisplayMember = nameof(CajaDto.Descripcion);

            _cmbTipo.Location = new Point(305, 18);
            _cmbTipo.Size = new Size(120, 28);
            _cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbTipo.DisplayMember = nameof(Opcion<TipoMovimiento?>.Texto);
            _cmbTipo.DataSource = new List<Opcion<TipoMovimiento?>>
            {
                new("Todos", null),
                new("Ingresos", TipoMovimiento.Ingreso),
                new("Egresos", TipoMovimiento.Egreso)
            };

            _cmbMedio.Location = new Point(440, 18);
            _cmbMedio.Size = new Size(140, 28);
            _cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbMedio.DisplayMember = nameof(Opcion<int?>.Texto);

            _txtBuscar.Location = new Point(595, 18);

            _cmbCaja.SelectedIndexChanged += (_, _) => Recargar();
            _cmbTipo.SelectedIndexChanged += (_, _) => Recargar();
            _cmbMedio.SelectedIndexChanged += (_, _) => Recargar();
            _txtBuscar.TextChanged += (_, _) => Recargar();

            barra.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Caja", 10, 0), _cmbCaja,
                Ui.Etiqueta("Tipo", 305, 0), _cmbTipo,
                Ui.Etiqueta("Medio de pago", 440, 0), _cmbMedio,
                Ui.Etiqueta("Buscar detalle o usuario", 595, 0), _txtBuscar
            });

            Ui.AgregarColumna(_grilla, "Fecha y hora", nameof(MovimientoDto.Fecha), "dd/MM/yyyy HH:mm");
            Ui.AgregarColumna(_grilla, "Tipo", nameof(MovimientoDto.Tipo));
            Ui.AgregarColumna(_grilla, "Categoría / motivo", nameof(MovimientoDto.Clasificacion));
            Ui.AgregarColumna(_grilla, "Detalle", nameof(MovimientoDto.Detalle));
            Ui.AgregarColumna(_grilla, "Medio de pago", nameof(MovimientoDto.MedioPago));
            Ui.AgregarColumna(_grilla, "Monto", nameof(MovimientoDto.MontoConSigno), "C2");
            Ui.AgregarColumna(_grilla, "Usuario", nameof(MovimientoDto.Usuario));
            _grilla.DataBindingComplete += (_, _) => PintarFilas();

            // Orden de agregado: primero el que llena, después la barra superior.
            Controls.Add(_grilla);
            Controls.Add(barra);
        }

        public void Refrescar()
        {
            var seleccionada = (_cmbCaja.SelectedItem as CajaDto)?.Id;
            var medioSeleccionado = (_cmbMedio.SelectedItem as Opcion<int?>)?.Valor;

            _cargando = true;
            try
            {
                _cmbCaja.DataSource = _controller.ObtenerCajas().ToList();
                SeleccionarCaja(seleccionada);

                CargarMedios(medioSeleccionado);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }
            finally
            {
                _cargando = false;
            }

            Recargar();
        }

        /// <summary>Carga los medios de pago del catálogo (más "Todos") y mantiene el elegido si sigue existiendo.</summary>
        private void CargarMedios(int? medioId)
        {
            var opciones = new List<Opcion<int?>> { new("Todos", null) };
            opciones.AddRange(_catalogos.ObtenerMediosPago().Select(m => new Opcion<int?>(m.Nombre, m.Id)));

            _cmbMedio.DataSource = opciones;
            _cmbMedio.SelectedIndex = Math.Max(0, opciones.FindIndex(o => o.Valor == medioId));
        }

        /// <summary>Mantiene la caja elegida; si no había, prefiere la abierta (están ordenadas de la más reciente).</summary>
        private void SeleccionarCaja(int? cajaId)
        {
            for (var i = 0; i < _cmbCaja.Items.Count; i++)
            {
                if (_cmbCaja.Items[i] is CajaDto caja && (cajaId is null ? caja.Abierta : caja.Id == cajaId))
                {
                    _cmbCaja.SelectedIndex = i;
                    return;
                }
            }

            if (_cmbCaja.Items.Count > 0)
                _cmbCaja.SelectedIndex = 0;
        }

        private void Recargar()
        {
            if (_cargando) return;

            if (_cmbCaja.SelectedItem is not CajaDto caja)
            {
                _grilla.DataSource = new List<MovimientoDto>();
                return;
            }

            var tipo = ((Opcion<TipoMovimiento?>)_cmbTipo.SelectedItem!).Valor;
            var medioId = (_cmbMedio.SelectedItem as Opcion<int?>)?.Valor;

            try
            {
                _grilla.DataSource = _controller.Obtener(new MovimientoFiltroDto(caja.Id, tipo, medioId, _txtBuscar.Text)).ToList();
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
                if (fila.DataBoundItem is not MovimientoDto movimiento) continue;

                fila.DefaultCellStyle.BackColor = movimiento.Tipo == TipoMovimiento.Ingreso
                    ? Color.Honeydew
                    : Color.MistyRose;
            }
        }
    }
}
