using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>Diálogo para cargar los datos de una cuota nueva. No guarda: devuelve el DTO.</summary>
    public class GenerarCuotaDialog : Form
    {
        private readonly int _socioId;
        private readonly NumericUpDown _numAnio = new();
        private readonly NumericUpDown _numMonto = new();

        /// <summary>Datos cargados; solo tiene valor si el usuario confirmó con "Generar".</summary>
        public CuotaCrearDto? Datos { get; private set; }

        public GenerarCuotaDialog(int socioId)
        {
            _socioId = socioId;
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            Text = "Generar cuota";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(320, 175);

            _numAnio.Location = new Point(20, 40);
            _numAnio.Size = new Size(280, 27);
            _numAnio.Minimum = 2000;
            _numAnio.Maximum = 2100;
            _numAnio.Value = DateTime.Today.Year;

            _numMonto.Location = new Point(20, 95);
            _numMonto.Size = new Size(280, 27);
            _numMonto.DecimalPlaces = 2;
            _numMonto.Maximum = 100_000_000m;
            _numMonto.ThousandsSeparator = true;

            var btnGenerar = new Button { Text = "Generar", Location = new Point(130, 135), Size = new Size(80, 30) };
            btnGenerar.Click += (_, _) => Confirmar();

            var btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(220, 135),
                Size = new Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            AcceptButton = btnGenerar;
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Año", 20, 20), _numAnio,
                Ui.Etiqueta("Monto", 20, 75), _numMonto,
                btnGenerar, btnCancelar
            });
        }

        private void Confirmar()
        {
            Datos = new CuotaCrearDto(_socioId, _numMonto.Value, (int)_numAnio.Value);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
