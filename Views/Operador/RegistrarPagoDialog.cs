using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>
    /// Diálogo para elegir con qué medio se paga una cuota y, si el socio tiene responsable, si paga él.
    /// No guarda: devuelve la elección.
    /// </summary>
    public class RegistrarPagoDialog : Form
    {
        private readonly CuotaDto _cuota;
        private readonly IReadOnlyList<MedioPagoDto> _medios;
        private readonly string? _responsable;
        private readonly ComboBox _cmbMedio = new();
        private readonly CheckBox _chkPagaResponsable = new();

        /// <summary>Medio elegido; null si no hay ninguno para elegir.</summary>
        public MedioPagoDto? Medio => _cmbMedio.SelectedItem as MedioPagoDto;

        /// <summary>True si el pago lo hace el responsable del socio.</summary>
        public bool PagaResponsable => _responsable is not null && _chkPagaResponsable.Checked;

        /// <param name="responsable">Descripción del responsable del socio, o null si no tiene.</param>
        public RegistrarPagoDialog(CuotaDto cuota, IReadOnlyList<MedioPagoDto> medios, string? responsable = null)
        {
            _cuota = cuota;
            _medios = medios;
            _responsable = responsable;
            ConstruirInterfaz();
            Tema.Aplicar(this);
        }

        private void ConstruirInterfaz()
        {
            Text = "Registrar pago";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;

            var detalle = new Label
            {
                Text = $"{_cuota.Descripcion}\nMonto: {_cuota.Monto:C2}",
                Location = new Point(20, 15),
                Size = new Size(380, 45)
            };

            _cmbMedio.Location = new Point(20, 90);
            _cmbMedio.Size = new Size(380, 28);
            _cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            Ui.Llenar(_cmbMedio, _medios);

            var y = 130;
            if (_responsable is not null)
            {
                _chkPagaResponsable.Text = $"Paga su responsable: {_responsable}";
                _chkPagaResponsable.Location = new Point(20, y);
                _chkPagaResponsable.Size = new Size(380, 24);
                Controls.Add(_chkPagaResponsable);
                y += 34;
            }

            var btnRegistrar = new Button
            {
                Text = "Registrar pago",
                Location = new Point(190, y + 10),
                Size = new Size(120, 32),
                DialogResult = DialogResult.OK
            };

            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(320, y + 10),
                Size = new Size(80, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = btnRegistrar;
            CancelButton = btnCancelar;
            ClientSize = new Size(420, y + 60);

            Controls.AddRange(new Control[]
            {
                detalle, Ui.Etiqueta("Medio de pago", 20, 68), _cmbMedio, btnRegistrar, btnCancelar
            });
        }
    }
}
