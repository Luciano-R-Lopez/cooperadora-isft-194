using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Pide el período de los egresos a exportar. Por defecto, desde el 1° de enero hasta hoy.</summary>
    public class ExportarEgresosDialog : Form
    {
        private readonly CampoFecha _desde = new() { Location = new Point(20, 40), Width = 170 };
        private readonly CampoFecha _hasta = new() { Location = new Point(210, 40), Width = 170 };

        public DateTime Desde => _desde.Value;
        public DateTime Hasta => _hasta.Value;

        public ExportarEgresosDialog()
        {
            Text = "Exportar egresos por grupo";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(400, 175);

            _desde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            _hasta.Value = DateTime.Today;

            var explicacion = new Label
            {
                Text = "Arma un CSV para Excel con el total de cada grupo y el detalle de sus gastos.",
                Location = new Point(20, 80),
                Size = new Size(360, 40)
            };

            var btnExportar = new Button
            {
                Text = "Exportar",
                Location = new Point(190, 128),
                Size = new Size(100, 32),
                DialogResult = DialogResult.OK
            };
            var btnCancelar = Tema.Secundario(new Button
            {
                Text = "Cancelar",
                Location = new Point(300, 128),
                Size = new Size(80, 32),
                DialogResult = DialogResult.Cancel
            });

            AcceptButton = btnExportar;
            CancelButton = btnCancelar;
            Controls.AddRange(new Control[]
            {
                Ui.Etiqueta("Desde", 20, 20), _desde,
                Ui.Etiqueta("Hasta", 210, 20), _hasta,
                explicacion, btnExportar, btnCancelar
            });
            Tema.Aplicar(this);
            explicacion.ForeColor = Tema.TextoSecundario;
        }
    }
}
