namespace CooperadoraISFT94.Forms
{
    partial class FrmCaja
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
            lblEstadoCaja = new Label();
            btnVolver = new Button();
            tlpAccion = new TableLayoutPanel();
            lblModo = new Label();
            lblSaldo = new Label();
            numSaldo = new NumericUpDown();
            btnAccionCaja = new Button();
            lblAviso = new Label();
            tlpResumen = new TableLayoutPanel();
            lblTituloResumen = new Label();
            lblResumenTituloInicial = new Label();
            lblResumenDetalleInicial = new Label();
            lblResumenTituloIngresos = new Label();
            lblResumenDetalleIngresos = new Label();
            lblResumenTituloEgresos = new Label();
            lblResumenDetalleEgresos = new Label();
            lblResumenTituloEsperado = new Label();
            lblResumenDetalleEsperado = new Label();
            lblResumenTituloDiferencia = new Label();
            lblResumenDetalleDiferencia = new Label();
            lblDgvMedios = new Label();
            dgvMedios = new DataGridView();
            tlpRaiz.SuspendLayout();
            tlpAccion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSaldo).BeginInit();
            tlpResumen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMedios).BeginInit();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpRaiz.Controls.Add(lblEstadoCaja, 0, 0);
            tlpRaiz.SetColumnSpan(lblEstadoCaja, 2);
            tlpRaiz.Controls.Add(btnVolver, 1, 1);
            tlpRaiz.Controls.Add(tlpAccion, 0, 2);
            tlpRaiz.Controls.Add(tlpResumen, 1, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 30);
            tlpRaiz.RowCount = 3;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // lblEstadoCaja
            //
            lblEstadoCaja.Dock = DockStyle.Fill;
            lblEstadoCaja.Name = "lblEstadoCaja";
            lblEstadoCaja.Text = "Estado de la caja";
            //
            // btnVolver
            //
            btnVolver.Anchor = AnchorStyles.Right;
            btnVolver.Margin = new Padding(3, 10, 3, 10);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 38);
            btnVolver.TabIndex = 10;
            btnVolver.Text = "Volver";
            btnVolver.Click += btnVolver_Click;
            //
            // tlpAccion
            //
            tlpAccion.ColumnCount = 1;
            tlpAccion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAccion.Controls.Add(lblModo, 0, 0);
            tlpAccion.Controls.Add(lblSaldo, 0, 1);
            tlpAccion.Controls.Add(numSaldo, 0, 2);
            tlpAccion.Controls.Add(btnAccionCaja, 0, 3);
            tlpAccion.Controls.Add(lblAviso, 0, 4);
            tlpAccion.Dock = DockStyle.Fill;
            tlpAccion.Margin = new Padding(0, 0, 40, 0);
            tlpAccion.Name = "tlpAccion";
            tlpAccion.RowCount = 6;
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpAccion.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAccion.TabIndex = 0;
            //
            // lblModo
            //
            lblModo.AutoSize = true;
            lblModo.Margin = new Padding(3, 0, 3, 2);
            lblModo.Name = "lblModo";
            lblModo.Text = "Abrir caja";
            //
            // lblSaldo
            //
            lblSaldo.AutoSize = true;
            lblSaldo.Margin = new Padding(3, 8, 3, 3);
            lblSaldo.MaximumSize = new Size(420, 0);
            lblSaldo.Name = "lblSaldo";
            lblSaldo.Text = "Saldo inicial: *";
            //
            // numSaldo
            //
            numSaldo.DecimalPlaces = 2;
            numSaldo.Dock = DockStyle.Fill;
            numSaldo.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numSaldo.Name = "numSaldo";
            numSaldo.TabIndex = 0;
            numSaldo.ThousandsSeparator = true;
            numSaldo.ValueChanged += numSaldo_ValueChanged;
            //
            // btnAccionCaja
            //
            btnAccionCaja.Dock = DockStyle.Fill;
            btnAccionCaja.Margin = new Padding(3, 20, 3, 3);
            btnAccionCaja.Name = "btnAccionCaja";
            btnAccionCaja.Size = new Size(94, 42);
            btnAccionCaja.TabIndex = 1;
            btnAccionCaja.Text = "Abrir caja";
            btnAccionCaja.Click += btnAccionCaja_Click;
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(3, 10, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // tlpResumen
            //
            tlpResumen.ColumnCount = 2;
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tlpResumen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpResumen.Controls.Add(lblTituloResumen, 0, 0);
            tlpResumen.SetColumnSpan(lblTituloResumen, 2);
            tlpResumen.Controls.Add(lblResumenTituloInicial, 0, 1);
            tlpResumen.Controls.Add(lblResumenDetalleInicial, 1, 1);
            tlpResumen.Controls.Add(lblResumenTituloIngresos, 0, 2);
            tlpResumen.Controls.Add(lblResumenDetalleIngresos, 1, 2);
            tlpResumen.Controls.Add(lblResumenTituloEgresos, 0, 3);
            tlpResumen.Controls.Add(lblResumenDetalleEgresos, 1, 3);
            tlpResumen.Controls.Add(lblResumenTituloEsperado, 0, 4);
            tlpResumen.Controls.Add(lblResumenDetalleEsperado, 1, 4);
            tlpResumen.Controls.Add(lblResumenTituloDiferencia, 0, 5);
            tlpResumen.Controls.Add(lblResumenDetalleDiferencia, 1, 5);
            tlpResumen.Controls.Add(lblDgvMedios, 0, 6);
            tlpResumen.SetColumnSpan(lblDgvMedios, 2);
            tlpResumen.Controls.Add(dgvMedios, 0, 7);
            tlpResumen.SetColumnSpan(dgvMedios, 2);
            tlpResumen.Dock = DockStyle.Fill;
            tlpResumen.Name = "tlpResumen";
            tlpResumen.RowCount = 8;
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpResumen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpResumen.TabIndex = 1;
            //
            // lblTituloResumen
            //
            lblTituloResumen.AutoSize = true;
            lblTituloResumen.Margin = new Padding(3, 0, 3, 8);
            lblTituloResumen.Name = "lblTituloResumen";
            lblTituloResumen.Text = "Resumen de la caja abierta";
            //
            // lblResumenTituloInicial
            //
            lblResumenTituloInicial.AutoSize = true;
            lblResumenTituloInicial.Margin = new Padding(3, 6, 3, 6);
            lblResumenTituloInicial.Name = "lblResumenTituloInicial";
            lblResumenTituloInicial.Text = "Saldo inicial";
            //
            // lblResumenDetalleInicial
            //
            lblResumenDetalleInicial.Anchor = AnchorStyles.Right;
            lblResumenDetalleInicial.AutoSize = true;
            lblResumenDetalleInicial.Name = "lblResumenDetalleInicial";
            lblResumenDetalleInicial.Text = "-";
            //
            // lblResumenTituloIngresos
            //
            lblResumenTituloIngresos.AutoSize = true;
            lblResumenTituloIngresos.Margin = new Padding(3, 6, 3, 6);
            lblResumenTituloIngresos.Name = "lblResumenTituloIngresos";
            lblResumenTituloIngresos.Text = "Total ingresos (+)";
            //
            // lblResumenDetalleIngresos
            //
            lblResumenDetalleIngresos.Anchor = AnchorStyles.Right;
            lblResumenDetalleIngresos.AutoSize = true;
            lblResumenDetalleIngresos.Name = "lblResumenDetalleIngresos";
            lblResumenDetalleIngresos.Text = "-";
            //
            // lblResumenTituloEgresos
            //
            lblResumenTituloEgresos.AutoSize = true;
            lblResumenTituloEgresos.Margin = new Padding(3, 6, 3, 6);
            lblResumenTituloEgresos.Name = "lblResumenTituloEgresos";
            lblResumenTituloEgresos.Text = "Total egresos (-)";
            //
            // lblResumenDetalleEgresos
            //
            lblResumenDetalleEgresos.Anchor = AnchorStyles.Right;
            lblResumenDetalleEgresos.AutoSize = true;
            lblResumenDetalleEgresos.Name = "lblResumenDetalleEgresos";
            lblResumenDetalleEgresos.Text = "-";
            //
            // lblResumenTituloEsperado
            //
            lblResumenTituloEsperado.AutoSize = true;
            lblResumenTituloEsperado.Margin = new Padding(3, 6, 3, 6);
            lblResumenTituloEsperado.Name = "lblResumenTituloEsperado";
            lblResumenTituloEsperado.Text = "Saldo esperado";
            //
            // lblResumenDetalleEsperado
            //
            lblResumenDetalleEsperado.Anchor = AnchorStyles.Right;
            lblResumenDetalleEsperado.AutoSize = true;
            lblResumenDetalleEsperado.Name = "lblResumenDetalleEsperado";
            lblResumenDetalleEsperado.Text = "-";
            //
            // lblResumenTituloDiferencia
            //
            lblResumenTituloDiferencia.AutoSize = true;
            lblResumenTituloDiferencia.Margin = new Padding(3, 6, 3, 6);
            lblResumenTituloDiferencia.Name = "lblResumenTituloDiferencia";
            lblResumenTituloDiferencia.Text = "Diferencia con lo contado";
            //
            // lblResumenDetalleDiferencia
            //
            lblResumenDetalleDiferencia.Anchor = AnchorStyles.Right;
            lblResumenDetalleDiferencia.AutoSize = true;
            lblResumenDetalleDiferencia.Name = "lblResumenDetalleDiferencia";
            lblResumenDetalleDiferencia.Text = "-";
            //
            // lblDgvMedios
            //
            lblDgvMedios.AutoSize = true;
            lblDgvMedios.Margin = new Padding(3, 20, 3, 6);
            lblDgvMedios.Name = "lblDgvMedios";
            lblDgvMedios.Text = "Saldo por medio de pago";
            //
            // dgvMedios
            //
            dgvMedios.AllowUserToAddRows = false;
            dgvMedios.AllowUserToDeleteRows = false;
            dgvMedios.AllowUserToResizeRows = false;
            dgvMedios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMedios.Dock = DockStyle.Fill;
            dgvMedios.MultiSelect = false;
            dgvMedios.Name = "dgvMedios";
            dgvMedios.ReadOnly = true;
            dgvMedios.RowHeadersVisible = false;
            dgvMedios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMedios.TabIndex = 2;
            //
            // FrmCaja
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmCaja";
            Text = "Apertura y cierre de caja";
            WindowState = FormWindowState.Maximized;
            Load += FrmCaja_Load;
            tlpRaiz.ResumeLayout(false);
            tlpAccion.ResumeLayout(false);
            tlpAccion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSaldo).EndInit();
            tlpResumen.ResumeLayout(false);
            tlpResumen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMedios).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblEstadoCaja;
        private Button btnVolver;
        private TableLayoutPanel tlpAccion;
        private Label lblModo;
        private Label lblSaldo;
        private NumericUpDown numSaldo;
        private Button btnAccionCaja;
        private Label lblAviso;
        private TableLayoutPanel tlpResumen;
        private Label lblTituloResumen;
        private Label lblResumenTituloInicial;
        private Label lblResumenDetalleInicial;
        private Label lblResumenTituloIngresos;
        private Label lblResumenDetalleIngresos;
        private Label lblResumenTituloEgresos;
        private Label lblResumenDetalleEgresos;
        private Label lblResumenTituloEsperado;
        private Label lblResumenDetalleEsperado;
        private Label lblResumenTituloDiferencia;
        private Label lblResumenDetalleDiferencia;
        private Label lblDgvMedios;
        private DataGridView dgvMedios;
    }
}
