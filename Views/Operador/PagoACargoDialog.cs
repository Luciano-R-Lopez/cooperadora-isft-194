using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.DTOs.Socios;
using CooperadoraISFT94.Enums;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>
    /// Un responsable paga la cuota de uno de sus socios a cargo: se elige el socio (si tiene más de uno),
    /// la cuota pendiente y el medio de pago. No guarda: devuelve la elección.
    /// </summary>
    public class PagoACargoDialog : Form
    {
        private readonly IReadOnlyList<SocioDto> _aCargo;
        private readonly Func<int, IReadOnlyList<CuotaDto>> _cuotasDe;
        private readonly ComboBox _cmbSocio = new();
        private readonly ComboBox _cmbCuota = new();
        private readonly ComboBox _cmbMedio = new();
        private readonly Button _btnRegistrar = new();
        private readonly Label _lblSinCuotas = new() { Text = "Este socio no tiene cuotas pendientes.", AutoSize = true, Visible = false };

        public CuotaDto? Cuota => _cmbCuota.SelectedItem as CuotaDto;

        public MedioPagoDto? Medio => _cmbMedio.SelectedItem as MedioPagoDto;

        /// <param name="cuotasDe">Cuotas de un socio (por id); el diálogo muestra solo las pendientes.</param>
        public PagoACargoDialog(string responsable, IReadOnlyList<SocioDto> aCargo,
                                Func<int, IReadOnlyList<CuotaDto>> cuotasDe, IReadOnlyList<MedioPagoDto> medios)
        {
            _aCargo = aCargo;
            _cuotasDe = cuotasDe;
            ConstruirInterfaz(responsable, medios);
            Tema.Aplicar(this);
            CargarCuotas();
        }

        private void ConstruirInterfaz(string responsable, IReadOnlyList<MedioPagoDto> medios)
        {
            Text = "Pagar cuota de un socio a cargo";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 300);

            var titulo = new Label { Text = $"Paga: {responsable}", Location = new Point(20, 15), Size = new Size(400, 24) };

            Configurar(_cmbSocio, 65);
            _cmbSocio.DisplayMember = nameof(SocioDto.Descripcion);
            Ui.Llenar(_cmbSocio, _aCargo);
            // Con un solo socio a cargo no hay nada que elegir.
            _cmbSocio.Enabled = _aCargo.Count > 1;
            _cmbSocio.SelectedIndexChanged += (_, _) => CargarCuotas();

            Configurar(_cmbCuota, 125);
            _lblSinCuotas.Location = new Point(20, 155);
            _cmbCuota.DisplayMember = nameof(CuotaDto.DescripcionConMonto);
            _cmbCuota.SelectedIndexChanged += (_, _) => _btnRegistrar.Enabled = Cuota is not null && Medio is not null;

            Configurar(_cmbMedio, 185);
            Ui.Llenar(_cmbMedio, medios);

            _btnRegistrar.Text = "Registrar pago";
            _btnRegistrar.Location = new Point(190, 245);
            _btnRegistrar.Size = new Size(130, 32);
            _btnRegistrar.DialogResult = DialogResult.OK;

            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(330, 245),
                Size = new Size(90, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = _btnRegistrar;
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                titulo,
                Ui.Etiqueta("Socio a cargo", 20, 45), _cmbSocio,
                Ui.Etiqueta("Cuota pendiente", 20, 105), _cmbCuota, _lblSinCuotas,
                Ui.Etiqueta("Medio de pago", 20, 165), _cmbMedio,
                _btnRegistrar, btnCancelar
            });
        }

        private static void Configurar(ComboBox combo, int y)
        {
            combo.Location = new Point(20, y);
            combo.Size = new Size(400, 28);
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void CargarCuotas()
        {
            var pendientes = _cmbSocio.SelectedItem is SocioDto socio
                ? _cuotasDe(socio.Id).Where(c => c.Estado == EstadoCuota.Pendiente).OrderBy(c => c.Anio).ToList()
                : new List<CuotaDto>();

            Ui.Llenar(_cmbCuota, pendientes);
            _cmbCuota.Enabled = pendientes.Count > 0;
            _lblSinCuotas.Visible = pendientes.Count == 0;
            _btnRegistrar.Enabled = Cuota is not null && Medio is not null;
        }
    }
}
