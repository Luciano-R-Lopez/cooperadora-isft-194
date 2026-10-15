using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Auditorias;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Admin
{
    /// <summary>Registro de auditoría: quién hizo qué sobre qué entidad (solo lectura), con filtros.</summary>
    public class AuditoriaView : UserControl, IRefrescable
    {
        private readonly AuditoriaController _controller;
        private readonly DataGridView _grilla = Ui.CrearGrilla();

        private readonly CheckBox _chkDesde = new() { Text = "Desde", AutoSize = true };
        private readonly DateTimePicker _dtpDesde = new() { Format = DateTimePickerFormat.Short, Width = 120 };
        private readonly CheckBox _chkHasta = new() { Text = "Hasta", AutoSize = true };
        private readonly DateTimePicker _dtpHasta = new() { Format = DateTimePickerFormat.Short, Width = 120 };
        private readonly ComboBox _cmbAccion = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
        private readonly TextBox _txtBuscar = new() { Width = 220 };

        private bool _cargando;

        public AuditoriaView() : this(new AuditoriaController()) { }

        public AuditoriaView(AuditoriaController controller)
        {
            _controller = controller;
            ConstruirInterfaz();
            Refrescar();
        }

        private void ConstruirInterfaz()
        {
            Dock = DockStyle.Fill;

            _cargando = true;
            var acciones = new List<Opcion<AccionAuditoria?>> { new("Todas", null) };
            acciones.AddRange(Enum.GetValues<AccionAuditoria>().Select(a => new Opcion<AccionAuditoria?>(a.ToString(), a)));
            Ui.Llenar(_cmbAccion, acciones);
            _dtpDesde.Value = DateTime.Today.AddDays(-30);
            _dtpDesde.Enabled = false;
            _dtpHasta.Enabled = false;
            _cargando = false;

            _chkDesde.CheckedChanged += (_, _) => { _dtpDesde.Enabled = _chkDesde.Checked; Refrescar(); };
            _chkHasta.CheckedChanged += (_, _) => { _dtpHasta.Enabled = _chkHasta.Checked; Refrescar(); };
            _dtpDesde.ValueChanged += (_, _) => Refrescar();
            _dtpHasta.ValueChanged += (_, _) => Refrescar();
            _cmbAccion.SelectedIndexChanged += (_, _) => Refrescar();
            _txtBuscar.TextChanged += (_, _) => Refrescar();

            var filtros = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(5, 8, 5, 0), WrapContents = false };
            filtros.Controls.AddRange(new Control[]
            {
                _chkDesde, _dtpDesde, _chkHasta, _dtpHasta,
                Etiqueta("Acción"), _cmbAccion,
                Etiqueta("Buscar usuario, entidad o detalle"), _txtBuscar
            });

            var acciones2 = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 48, Padding = new Padding(0, 6, 0, 0) };
            var btnExportar = Tema.Secundario(new Button { Text = "Exportar a CSV", Size = new Size(140, 32) });
            btnExportar.Click += (_, _) => ExportarCsv.Grilla(_grilla, "Auditoría");
            acciones2.Controls.Add(btnExportar);

            Ui.AgregarColumna(_grilla, "Fecha y hora", nameof(AuditoriaDto.Fecha), "dd/MM/yyyy HH:mm:ss");
            Ui.AgregarColumna(_grilla, "Usuario", nameof(AuditoriaDto.Usuario));
            Ui.AgregarColumna(_grilla, "Acción", nameof(AuditoriaDto.Accion));
            Ui.AgregarColumna(_grilla, "Entidad", nameof(AuditoriaDto.EntidadAfectada));
            Ui.AgregarColumna(_grilla, "Id", nameof(AuditoriaDto.EntidadId));
            Ui.AgregarColumna(_grilla, "Detalle", nameof(AuditoriaDto.Detalle));
            _grilla.Columns[4].FillWeight = 30;
            _grilla.Columns[5].FillWeight = 250;

            // Orden de agregado: primero el que llena, después los bordes.
            Controls.Add(_grilla);
            Controls.Add(acciones2);
            Controls.Add(filtros);
        }

        private static Label Etiqueta(string texto) =>
            new() { Text = texto, AutoSize = true, Margin = new Padding(15, 6, 3, 0) };

        public void Refrescar()
        {
            if (_cargando) return;

            var filtro = new AuditoriaFiltroDto(
                _chkDesde.Checked ? _dtpDesde.Value.Date : null,
                _chkHasta.Checked ? _dtpHasta.Value.Date : null,
                (_cmbAccion.SelectedItem as Opcion<AccionAuditoria?>)?.Valor,
                _txtBuscar.Text);

            try
            {
                _grilla.DataSource = _controller.Obtener(filtro).ToList();
            }
            catch (NegocioException ex)
            {
                Ui.Error(ex.Message);
            }
        }
    }
}
