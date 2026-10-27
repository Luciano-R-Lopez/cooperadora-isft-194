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
        private readonly ComboBox _cmbGrupo = new();
        private readonly TextBox _txtBuscar = Ui.Campo(740, 18, 200);
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnAnular = new();

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

            _cmbGrupo.Location = new Point(595, 18);
            _cmbGrupo.Size = new Size(150, 28);
            _cmbGrupo.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbGrupo.DisplayMember = nameof(Opcion<int?>.Texto);

            _txtBuscar.Location = new Point(760, 18);

            _cmbCaja.SelectedIndexChanged += (_, _) => Recargar();
            _cmbTipo.SelectedIndexChanged += (_, _) => Recargar();
            _cmbMedio.SelectedIndexChanged += (_, _) => Recargar();
            _cmbGrupo.SelectedIndexChanged += (_, _) => Recargar();
            _txtBuscar.TextChanged += (_, _) => Recargar();

            barra.Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Caja", 10, 0), _cmbCaja,
                Ui.Etiqueta("Tipo", 305, 0), _cmbTipo,
                Ui.Etiqueta("Medio de pago", 440, 0), _cmbMedio,
                Ui.Etiqueta("Grupo de egreso", 595, 0), _cmbGrupo,
                Ui.Etiqueta("Buscar detalle o usuario", 760, 0), _txtBuscar
            });

            Ui.AgregarColumna(_grilla, "Fecha y hora", nameof(MovimientoDto.Fecha), "dd/MM/yyyy HH:mm");
            Ui.AgregarColumna(_grilla, "Tipo", nameof(MovimientoDto.Tipo));
            Ui.AgregarColumna(_grilla, "Categoría / motivo", nameof(MovimientoDto.Clasificacion));
            Ui.AgregarColumna(_grilla, "Grupo", nameof(MovimientoDto.Grupo));
            Ui.AgregarColumna(_grilla, "Detalle", nameof(MovimientoDto.Detalle));
            Ui.AgregarColumna(_grilla, "Medio de pago", nameof(MovimientoDto.MedioPago));
            Ui.AgregarColumna(_grilla, "Monto", nameof(MovimientoDto.MontoConSigno), "C2");
            Ui.AgregarColumna(_grilla, "Usuario", nameof(MovimientoDto.Usuario));
            Ui.AgregarColumna(_grilla, "Estado", nameof(MovimientoDto.EstadoTexto));
            _grilla.DataBindingComplete += (_, _) => PintarFilas();
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();

            var acciones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 6, 0, 0) };

            _btnAnular.Text = "Anular movimiento";
            _btnAnular.Size = new Size(170, 32);
            _btnAnular.Click += (_, _) => AnularSeleccionado();

            var btnImprimir = Tema.Secundario(new Button { Text = "Imprimir resumen de caja", Size = new Size(200, 32) });
            btnImprimir.Click += (_, _) => ImprimirResumen();

            var btnExportar = Tema.Secundario(new Button { Text = "Exportar a CSV", Size = new Size(140, 32) });
            btnExportar.Click += (_, _) => ExportarCsv.Grilla(_grilla, "Movimientos");

            acciones.Controls.AddRange(new Control[] { _btnAnular, btnImprimir, btnExportar });

            // Orden de agregado: primero el que llena, después la barra superior.
            Controls.Add(_grilla);
            Controls.Add(acciones);
            Controls.Add(barra);
        }

        public void Refrescar()
        {
            var seleccionada = (_cmbCaja.SelectedItem as CajaDto)?.Id;
            var medioSeleccionado = (_cmbMedio.SelectedItem as Opcion<int?>)?.Valor;
            var grupoSeleccionado = (_cmbGrupo.SelectedItem as Opcion<int?>)?.Valor;

            _cargando = true;
            try
            {
                _cmbCaja.DataSource = _controller.ObtenerCajas().ToList();
                SeleccionarCaja(seleccionada);

                CargarMedios(medioSeleccionado);
                CargarGrupos(grupoSeleccionado);
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
            opciones.AddRange(_catalogos.ObtenerMediosPago(incluirInactivos: true).Select(m => new Opcion<int?>(m.Nombre, m.Id)));

            _cmbMedio.DataSource = opciones;
            _cmbMedio.SelectedIndex = Math.Max(0, opciones.FindIndex(o => o.Valor == medioId));
        }

        /// <summary>Carga los grupos de egreso (más "Todos") y mantiene el elegido si sigue existiendo.</summary>
        private void CargarGrupos(int? grupoId)
        {
            var opciones = new List<Opcion<int?>> { new("Todos", null) };
            opciones.AddRange(_catalogos.ObtenerGruposEgreso(incluirInactivos: true).Select(g => new Opcion<int?>(g.Nombre, g.Id)));

            _cmbGrupo.DataSource = opciones;
            _cmbGrupo.SelectedIndex = Math.Max(0, opciones.FindIndex(o => o.Valor == grupoId));
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
                ActualizarBotones();
                return;
            }

            var tipo = ((Opcion<TipoMovimiento?>)_cmbTipo.SelectedItem!).Valor;
            var medioId = (_cmbMedio.SelectedItem as Opcion<int?>)?.Valor;
            var grupoId = (_cmbGrupo.SelectedItem as Opcion<int?>)?.Valor;

            try
            {
                _grilla.DataSource = _controller.Obtener(new MovimientoFiltroDto(caja.Id, tipo, medioId, _txtBuscar.Text, grupoId))
                                                .ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }

            ActualizarBotones();
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is not MovimientoDto movimiento) continue;

                // Los anulados quedan a la vista pero apagados: ya no cuentan en la caja.
                if (movimiento.Anulado)
                {
                    fila.DefaultCellStyle.BackColor = Tema.Superficie;
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
                    continue;
                }

                fila.DefaultCellStyle.BackColor = movimiento.Tipo == TipoMovimiento.Ingreso
                    ? Tema.ExitoSuave
                    : Tema.PeligroSuave;
            }
        }

        private MovimientoDto? MovimientoSeleccionado() => _grilla.CurrentRow?.DataBoundItem as MovimientoDto;

        /// <summary>Solo se anulan movimientos vigentes de la caja abierta.</summary>
        private void ActualizarBotones() =>
            _btnAnular.Enabled = _cmbCaja.SelectedItem is CajaDto { Abierta: true }
                                 && MovimientoSeleccionado() is { Anulado: false };

        private void AnularSeleccionado()
        {
            if (MovimientoSeleccionado() is not MovimientoDto movimiento)
                return;

            var motivo = DialogoTexto.Pedir(this, "Anular movimiento",
                $"Se va a anular {movimiento.Tipo.ToString().ToLowerInvariant()} de {movimiento.Monto:C2} ({movimiento.Clasificacion}). Motivo:",
                "Anular");
            if (motivo is null)
                return;

            try
            {
                _controller.Anular(movimiento.Id, motivo);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            Recargar();
            Ui.Info("Movimiento anulado. Se revirtió su efecto en la caja.");
        }

        private void ImprimirResumen()
        {
            if (_cmbCaja.SelectedItem is not CajaDto caja)
            {
                Ui.Error("Elegí una caja.");
                return;
            }

            try
            {
                var resumen = _controller.ObtenerResumen(caja.Id);
                var movimientos = _controller.Obtener(new MovimientoFiltroDto(caja.Id, null, null, null));
                Reportes.ImprimirResumenCaja(resumen, movimientos);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }
    }
}
