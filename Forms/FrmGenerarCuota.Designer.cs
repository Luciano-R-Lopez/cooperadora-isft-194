namespace CooperadoraISFT94.Forms
{
    partial class FrmGenerarCuota
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
            lblAnio = new Label();
            numAnio = new NumericUpDown();
            lblMonto = new Label();
            numMonto = new NumericUpDown();
            lblAyuda = new Label();
            flpBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnGenerar = new Button();
            tlpRaiz.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAnio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMonto).BeginInit();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.Controls.Add(lblAnio, 0, 0);
            tlpRaiz.Controls.Add(numAnio, 0, 1);
            tlpRaiz.Controls.Add(lblMonto, 0, 2);
            tlpRaiz.Controls.Add(numMonto, 0, 3);
            tlpRaiz.Controls.Add(lblAyuda, 0, 4);
            tlpRaiz.Controls.Add(flpBotones, 0, 5);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 16, 24, 16);
            tlpRaiz.RowCount = 6;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblAnio
            //
            lblAnio.AutoSize = true;
            lblAnio.Margin = new Padding(3, 8, 3, 3);
            lblAnio.Name = "lblAnio";
            lblAnio.Text = "Año: *";
            //
            // numAnio
            //
            numAnio.Dock = DockStyle.Fill;
            numAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            numAnio.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            numAnio.Name = "numAnio";
            numAnio.TabIndex = 0;
            numAnio.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            numAnio.ValueChanged += numAnio_ValueChanged;
            //
            // lblMonto
            //
            lblMonto.AutoSize = true;
            lblMonto.Margin = new Padding(3, 8, 3, 3);
            lblMonto.Name = "lblMonto";
            lblMonto.Text = "Monto: *";
            //
            // numMonto
            //
            numMonto.DecimalPlaces = 2;
            numMonto.Dock = DockStyle.Fill;
            numMonto.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numMonto.Name = "numMonto";
            numMonto.TabIndex = 1;
            numMonto.ThousandsSeparator = true;
            //
            // lblAyuda
            //
            lblAyuda.AutoSize = true;
            lblAyuda.Margin = new Padding(3, 8, 3, 3);
            lblAyuda.MaximumSize = new Size(380, 0);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Text = "Se propone el valor de la cuota fijado para el año elegido.";
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnGenerar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Margin = new Padding(3, 12, 3, 3);
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
            // btnGenerar
            //
            btnGenerar.Margin = new Padding(8, 0, 0, 0);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(120, 40);
            btnGenerar.TabIndex = 0;
            btnGenerar.Text = "Generar";
            btnGenerar.Click += btnGenerar_Click;
            //
            // FrmGenerarCuota
            //
            AcceptButton = btnGenerar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(440, 320);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmGenerarCuota";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Generar cuota";
            Load += FrmGenerarCuota_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAnio).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMonto).EndInit();
            flpBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblAnio;
        private NumericUpDown numAnio;
        private Label lblMonto;
        private NumericUpDown numMonto;
        private Label lblAyuda;
        private FlowLayoutPanel flpBotones;
        private Button btnCancelar;
        private Button btnGenerar;
    }
}
