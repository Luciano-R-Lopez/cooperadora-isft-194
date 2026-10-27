namespace CooperadoraISFT94.Forms
{
    partial class FrmIngresos
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
            tlpFormulario = new TableLayoutPanel();
            lblModo = new Label();
            lblMonto = new Label();
            numMonto = new NumericUpDown();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            lblMedio = new Label();
            cmbMedio = new ComboBox();
            btnRegistrar = new Button();
            lblAviso = new Label();
            lblAyuda = new Label();
            tlpRaiz.SuspendLayout();
            tlpFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMonto).BeginInit();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 3;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 460F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.Controls.Add(lblEstadoCaja, 0, 0);
            tlpRaiz.SetColumnSpan(lblEstadoCaja, 3);
            tlpRaiz.Controls.Add(btnVolver, 2, 1);
            tlpRaiz.Controls.Add(tlpFormulario, 1, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 30);
            tlpRaiz.RowCount = 4;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
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
            // tlpFormulario
            //
            tlpFormulario.AutoSize = true;
            tlpFormulario.ColumnCount = 1;
            tlpFormulario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFormulario.Controls.Add(lblModo, 0, 0);
            tlpFormulario.Controls.Add(lblMonto, 0, 1);
            tlpFormulario.Controls.Add(numMonto, 0, 2);
            tlpFormulario.Controls.Add(lblCategoria, 0, 3);
            tlpFormulario.Controls.Add(cmbCategoria, 0, 4);
            tlpFormulario.Controls.Add(lblMedio, 0, 5);
            tlpFormulario.Controls.Add(cmbMedio, 0, 6);
            tlpFormulario.Controls.Add(btnRegistrar, 0, 7);
            tlpFormulario.Controls.Add(lblAviso, 0, 8);
            tlpFormulario.Controls.Add(lblAyuda, 0, 9);
            tlpFormulario.Dock = DockStyle.Fill;
            tlpFormulario.Margin = new Padding(0, 20, 0, 0);
            tlpFormulario.Name = "tlpFormulario";
            tlpFormulario.RowCount = 10;
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.TabIndex = 0;
            //
            // lblModo
            //
            lblModo.AutoSize = true;
            lblModo.Margin = new Padding(3, 0, 3, 2);
            lblModo.Name = "lblModo";
            lblModo.Text = "Nueva donación o subsidio";
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
            numMonto.TabIndex = 0;
            numMonto.ThousandsSeparator = true;
            //
            // lblCategoria
            //
            lblCategoria.AutoSize = true;
            lblCategoria.Margin = new Padding(3, 8, 3, 3);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Text = "Categoría: *";
            //
            // cmbCategoria
            //
            cmbCategoria.Dock = DockStyle.Fill;
            cmbCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoria.Name = "cmbCategoria";
            cmbCategoria.TabIndex = 1;
            //
            // lblMedio
            //
            lblMedio.AutoSize = true;
            lblMedio.Margin = new Padding(3, 8, 3, 3);
            lblMedio.Name = "lblMedio";
            lblMedio.Text = "Medio de pago: *";
            //
            // cmbMedio
            //
            cmbMedio.Dock = DockStyle.Fill;
            cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedio.Name = "cmbMedio";
            cmbMedio.TabIndex = 2;
            //
            // btnRegistrar
            //
            btnRegistrar.Dock = DockStyle.Fill;
            btnRegistrar.Margin = new Padding(3, 20, 3, 3);
            btnRegistrar.Name = "btnRegistrar";
            btnRegistrar.Size = new Size(94, 42);
            btnRegistrar.TabIndex = 3;
            btnRegistrar.Text = "Registrar ingreso";
            btnRegistrar.Click += btnRegistrar_Click;
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(3, 10, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // lblAyuda
            //
            lblAyuda.AutoSize = true;
            lblAyuda.Margin = new Padding(3, 16, 3, 0);
            lblAyuda.MaximumSize = new Size(440, 0);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Text = "Las cuotas sociales se cobran desde Cuotas y morosos o desde la ficha del socio.";
            //
            // FrmIngresos
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(800, 600);
            Name = "FrmIngresos";
            Text = "Donaciones y subsidios";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmIngresos_FormClosing;
            Load += FrmIngresos_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            tlpFormulario.ResumeLayout(false);
            tlpFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numMonto).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblEstadoCaja;
        private Button btnVolver;
        private TableLayoutPanel tlpFormulario;
        private Label lblModo;
        private Label lblMonto;
        private NumericUpDown numMonto;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Label lblMedio;
        private ComboBox cmbMedio;
        private Button btnRegistrar;
        private Label lblAviso;
        private Label lblAyuda;
    }
}
