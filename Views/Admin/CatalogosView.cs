using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Admin
{
    /// <summary>
    /// Mantenimiento de los catálogos (medios de pago, motivos y grupos de egreso): agregar, renombrar y
    /// desactivar. Un ítem desactivado no se ofrece para movimientos nuevos; los registrados no cambian.
    /// </summary>
    public class CatalogosView : UserControl, IRefrescable
    {
        private readonly CatalogoController _controller;

        private readonly ComboBox _cmbCatalogo = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 200 };
        private readonly DataGridView _grilla = Ui.CrearGrilla();
        private readonly Button _btnRenombrar = new();
        private readonly Button _btnEstado = new();

        public CatalogosView() : this(new CatalogoController()) { }

        public CatalogosView(CatalogoController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Refrescar();
        }

        private TipoCatalogo Tipo => ((Opcion<TipoCatalogo>)_cmbCatalogo.SelectedItem!).Valor;

        private string NombreCatalogo => Tipo switch
        {
            TipoCatalogo.MedioPago => "medio de pago",
            TipoCatalogo.MotivoEgreso => "motivo de egreso",
            _ => "grupo de egreso"
        };

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            Ui.Llenar(_cmbCatalogo, new List<Opcion<TipoCatalogo>>
            {
                new("Medios de pago", TipoCatalogo.MedioPago),
                new("Motivos de egreso", TipoCatalogo.MotivoEgreso),
                new("Grupos de egreso", TipoCatalogo.GrupoEgreso)
            });
            _cmbCatalogo.SelectedIndexChanged += (_, _) => Refrescar();

            var arriba = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(0, 8, 0, 0) };
            arriba.Controls.AddRange(new Control[]
            {
                new Label { Text = "Catálogo", AutoSize = true, Margin = new Padding(5, 8, 5, 0) }, _cmbCatalogo
            });

            var abajo = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 6, 0, 0) };

            var btnAgregar = new Button { Text = "Agregar", Size = new Size(120, 32) };
            btnAgregar.Click += (_, _) => Agregar();

            _btnRenombrar.Text = "Renombrar";
            _btnRenombrar.Size = new Size(120, 32);
            _btnRenombrar.Click += (_, _) => Renombrar();

            _btnEstado.Size = new Size(120, 32);
            _btnEstado.Click += (_, _) => CambiarEstado();

            abajo.Controls.AddRange(new Control[] { btnAgregar, _btnRenombrar, _btnEstado });

            Ui.AgregarColumna(_grilla, "Nombre", nameof(ItemCatalogoDto.Nombre));
            Ui.AgregarColumna(_grilla, "Estado", nameof(ItemCatalogoDto.EstadoTexto));
            _grilla.SelectionChanged += (_, _) => ActualizarBotones();
            _grilla.DataBindingComplete += (_, _) => PintarFilas();
            _grilla.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) Renombrar(); };

            // Orden de agregado: primero el que llena, después los bordes.
            Controls.Add(_grilla);
            Controls.Add(abajo);
            Controls.Add(arriba);
        }

        public void Refrescar() => Cargar();

        private void Cargar(int? seleccionarId = null)
        {
            try
            {
                _grilla.DataSource = _controller.ObtenerItems(Tipo).ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
                return;
            }

            if (seleccionarId is not null)
            {
                foreach (DataGridViewRow fila in _grilla.Rows)
                {
                    if (fila.DataBoundItem is ItemCatalogoDto i && i.Id == seleccionarId)
                    {
                        fila.Selected = true;
                        _grilla.CurrentCell = fila.Cells[0];
                        break;
                    }
                }
            }

            ActualizarBotones();
        }

        private ItemCatalogoDto? Seleccionado() => _grilla.CurrentRow?.DataBoundItem as ItemCatalogoDto;

        private void ActualizarBotones()
        {
            var item = Seleccionado();
            _btnRenombrar.Enabled = item is not null;
            _btnEstado.Enabled = item is not null;
            _btnEstado.Text = item is { Activo: false } ? "Activar" : "Desactivar";
        }

        private void PintarFilas()
        {
            foreach (DataGridViewRow fila in _grilla.Rows)
            {
                if (fila.DataBoundItem is ItemCatalogoDto { Activo: false })
                    fila.DefaultCellStyle.ForeColor = Tema.TextoSecundario;
            }
        }

        private void Agregar()
        {
            var nombre = DialogoTexto.Pedir(this, "Agregar", $"Nombre del nuevo {NombreCatalogo}:", "Agregar");
            if (nombre is null) return;

            Ejecutar(() => _controller.Agregar(Tipo, nombre));
        }

        private void Renombrar()
        {
            if (Seleccionado() is not ItemCatalogoDto item) return;

            var nombre = DialogoTexto.Pedir(this, "Renombrar", $"Nuevo nombre para \"{item.Nombre}\":", "Renombrar");
            if (nombre is null) return;

            Ejecutar(() => _controller.Renombrar(Tipo, item.Id, nombre));
        }

        private void CambiarEstado()
        {
            if (Seleccionado() is not ItemCatalogoDto item) return;

            if (item.Activo && !Ui.Confirmar(
                    $"¿Desactivar \"{item.Nombre}\"? Ya no se va a poder elegir en movimientos nuevos; los registrados no cambian."))
                return;

            Ejecutar(() => _controller.CambiarEstado(Tipo, item.Id, !item.Activo));
        }

        private void Ejecutar(Func<ItemCatalogoDto> accion)
        {
            try
            {
                Cargar(accion().Id);
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }
    }
}
