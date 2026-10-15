using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>Diálogo para elegir con qué medio se paga una cuota. No guarda: devuelve la elección.</summary>
    public class RegistrarPagoDialog : Form
    {
        private readonly CuotaDto _cuota;
        private readonly IReadOnlyList<MedioPagoDto> _medios;
        private readonly ComboBox _cmbMedio = new();

        /// <summary>Medio elegido; null si no hay ninguno para elegir.</summary>
        public MedioPagoDto? Medio => _cmbMedio.SelectedItem as MedioPagoDto;

        public RegistrarPagoDialog(CuotaDto cuota, IReadOnlyList<MedioPagoDto> medios)
        {
            _cuota = cuota;
            _medios = medios;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Text = "Registrar pago";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(340, 190);

            var detalle = new Label
            {
                Text = $"{_cuota.Descripcion}\nMonto: {_cuota.Monto:C2}",
                Location = new Point(20, 15),
                Size = new Size(300, 45)
            };

            _cmbMedio.Location = new Point(20, 90);
            _cmbMedio.Size = new Size(300, 28);
            _cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbMedio.DataSource = _medios.ToList();

            var btnRegistrar = new Button
            {
                Text = "Registrar pago",
                Location = new Point(110, 140),
                Size = new Size(120, 32),
                DialogResult = DialogResult.OK
            };

            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(240, 140),
                Size = new Size(80, 32),
                DialogResult = DialogResult.Cancel
            };

            AcceptButton = btnRegistrar;
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                detalle, Ui.Etiqueta("Medio de pago", 20, 68), _cmbMedio, btnRegistrar, btnCancelar
            });
        }
    }
}
