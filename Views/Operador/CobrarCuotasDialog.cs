using CooperadoraISFT94.DTOs.Catalogos;
using CooperadoraISFT94.DTOs.Cuotas;
using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Operador
{
    /// <summary>
    /// Cobro de una o varias cuotas pendientes de un socio en un solo pago (un ingreso y un recibo).
    /// No guarda: devuelve la elección.
    /// </summary>
    public class CobrarCuotasDialog : Form
    {
        private readonly CheckedListBox _lstCuotas = new() { CheckOnClick = true, BorderStyle = BorderStyle.FixedSingle };
        private readonly ComboBox _cmbMedio = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly CheckBox _chkPagaResponsable = new();
        private readonly Label _lblTotal = new() { AutoSize = true };
        private readonly Button _btnCobrar = new();

        /// <summary>Cuotas tildadas.</summary>
        public IReadOnlyList<CuotaDto> Cuotas => _lstCuotas.CheckedItems.Cast<CuotaDto>().ToList();

        public MedioPagoDto? Medio => _cmbMedio.SelectedItem as MedioPagoDto;

        /// <summary>True si el pago lo hace el responsable del socio.</summary>
        public bool PagaResponsable => _chkPagaResponsable.Visible && _chkPagaResponsable.Checked;

        /// <param name="responsable">Descripción del responsable del socio, o null si no tiene.</param>
        public CobrarCuotasDialog(string socio, IReadOnlyList<CuotaDto> pendientes, IReadOnlyList<MedioPagoDto> medios,
                                  string? responsable)
        {
            Text = "Cobrar cuotas";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(440, 380);

            var titulo = new Label { Text = socio, Location = new Point(20, 15), Size = new Size(400, 24), Font = new Font(Font, FontStyle.Bold) };

            _lstCuotas.Location = new Point(20, 68);
            _lstCuotas.Size = new Size(400, 110);
            _lstCuotas.DisplayMember = nameof(CuotaDto.DescripcionConMonto);
            foreach (var cuota in pendientes.OrderBy(c => c.Anio))
                _lstCuotas.Items.Add(cuota, isChecked: true);
            // ItemCheck avisa antes de que cambie el tilde: se recalcula cuando ya cambió.
            _lstCuotas.ItemCheck += (_, _) => BeginInvoke(ActualizarTotal);

            _lblTotal.Location = new Point(20, 186);

            _cmbMedio.Location = new Point(20, 238);
            _cmbMedio.Size = new Size(400, 28);
            Ui.Llenar(_cmbMedio, medios);

            _chkPagaResponsable.Text = $"Paga su responsable: {responsable}";
            _chkPagaResponsable.Location = new Point(20, 280);
            _chkPagaResponsable.Size = new Size(400, 24);
            _chkPagaResponsable.Visible = responsable is not null;
            // Si tiene responsable, lo habitual es que pague él.
            _chkPagaResponsable.Checked = responsable is not null;

            _btnCobrar.Text = "Cobrar";
            _btnCobrar.Location = new Point(230, 326);
            _btnCobrar.Size = new Size(100, 32);
            _btnCobrar.DialogResult = DialogResult.OK;

            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(340, 326),
                Size = new Size(80, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = _btnCobrar;
            CancelButton = btnCancelar;

            Controls.AddRange(new Control[]
            {
                titulo,
                Ui.Etiqueta("Cuotas a cobrar", 20, 48), _lstCuotas, _lblTotal,
                Ui.Etiqueta("Medio de pago", 20, 218), _cmbMedio,
                _chkPagaResponsable, _btnCobrar, btnCancelar
            });

            Tema.Aplicar(this);
            _lblTotal.Font = new Font(Font, FontStyle.Bold);
            ActualizarTotal();
        }

        private void ActualizarTotal()
        {
            var elegidas = Cuotas;
            _lblTotal.Text = elegidas.Count == 0
                ? "Elegí al menos una cuota."
                : $"Total a cobrar: {elegidas.Sum(c => c.Monto):C2} ({elegidas.Count} cuota{(elegidas.Count == 1 ? "" : "s")})";
            _btnCobrar.Enabled = elegidas.Count > 0 && Medio is not null;
        }
    }
}
