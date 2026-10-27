namespace CooperadoraISFT94.Forms
{
    partial class FrmExportarEgresos
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tlpRaiz = new TableLayoutPanel();
            lblDesde = new Label();
            lblHasta = new Label();
            dtpDesde = new Utils.CampoFecha();
            dtpHasta = new Utils.CampoFecha();
            lblAyuda = new Label();
            flpBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnAceptar = new Button();
            tlpRaiz.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.Controls.Add(lblDesde, 0, 0);
            tlpRaiz.Controls.Add(lblHasta, 1, 0);
            tlpRaiz.Controls.Add(dtpDesde, 0, 1);
            tlpRaiz.Controls.Add(dtpHasta, 1, 1);
            tlpRaiz.Controls.Add(lblAyuda, 0, 2);
            tlpRaiz.SetColumnSpan(lblAyuda, 2);
            tlpRaiz.Controls.Add(flpBotones, 0, 3);
            tlpRaiz.SetColumnSpan(flpBotones, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 20, 24, 16);
            tlpRaiz.RowCount = 4;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // lblDesde
            //
            lblDesde.AutoSize = true;
            lblDesde.Margin = new Padding(3, 0, 3, 3);
            lblDesde.Name = "lblDesde";
            lblDesde.Text = "Desde: *";
            //
            // lblHasta
            //
            lblHasta.AutoSize = true;
            lblHasta.Margin = new Padding(9, 0, 3, 3);
            lblHasta.Name = "lblHasta";
            lblHasta.Text = "Hasta: *";
            //
            // dtpDesde
            //
            dtpDesde.Dock = DockStyle.Fill;
            dtpDesde.Margin = new Padding(3, 3, 6, 3);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.TabIndex = 0;
            //
            // dtpHasta
            //
            dtpHasta.Dock = DockStyle.Fill;
            dtpHasta.Margin = new Padding(9, 3, 3, 3);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.TabIndex = 1;
            //
            // lblAyuda
            //
            lblAyuda.AutoSize = true;
            lblAyuda.Margin = new Padding(3, 14, 3, 3);
            lblAyuda.MaximumSize = new Size(420, 0);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Text = "Se arma un CSV para Excel con el total de cada grupo y el detalle de sus gastos. No incluye los egresos anulados.";
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnAceptar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Name = "flpBotones";
            flpBotones.TabIndex = 2;
            flpBotones.WrapContents = false;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            //
            // btnAceptar
            //
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Margin = new Padding(8, 0, 0, 0);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 40);
            btnAceptar.TabIndex = 0;
            btnAceptar.Text = "Exportar";
            //
            // FrmExportarEgresos
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(480, 250);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmExportarEgresos";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Exportar egresos por grupo";
            Load += FrmExportarEgresos_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblDesde;
        private Label lblHasta;
        private Utils.CampoFecha dtpDesde;
        private Utils.CampoFecha dtpHasta;
        private Label lblAyuda;
        private FlowLayoutPanel flpBotones;
        private Button btnCancelar;
        private Button btnAceptar;
    }
}
