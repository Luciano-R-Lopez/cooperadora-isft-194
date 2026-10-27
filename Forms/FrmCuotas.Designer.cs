namespace CooperadoraISFT94.Forms
{
    partial class FrmCuotas
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
            lblTituloCuotaAnio = new Label();
            btnVolver = new Button();
            flpCuotaAnio = new FlowLayoutPanel();
            lblAnio = new Label();
            numAnio = new NumericUpDown();
            lblValor = new Label();
            numValor = new NumericUpDown();
            btnGuardarValor = new Button();
            btnGenerar = new Button();
            lblDgvMorosos = new Label();
            flpFiltros = new FlowLayoutPanel();
            lblAnioMorosos = new Label();
            cmbAnioMorosos = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            chkDadosDeBaja = new CheckBox();
            dgvMorosos = new DataGridView();
            flpAcciones = new FlowLayoutPanel();
            btnCobrar = new Button();
            btnVerFicha = new Button();
            btnCopiarEmails = new Button();
            btnExportar = new Button();
            lblTotalMorosos = new Label();
            lblAviso = new Label();
            tlpRaiz.SuspendLayout();
            flpCuotaAnio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numAnio).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numValor).BeginInit();
            flpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMorosos).BeginInit();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.Controls.Add(lblTituloCuotaAnio, 0, 0);
            tlpRaiz.Controls.Add(btnVolver, 1, 0);
            tlpRaiz.Controls.Add(flpCuotaAnio, 0, 1);
            tlpRaiz.SetColumnSpan(flpCuotaAnio, 2);
            tlpRaiz.Controls.Add(lblDgvMorosos, 0, 2);
            tlpRaiz.SetColumnSpan(lblDgvMorosos, 2);
            tlpRaiz.Controls.Add(flpFiltros, 0, 3);
            tlpRaiz.SetColumnSpan(flpFiltros, 2);
            tlpRaiz.Controls.Add(dgvMorosos, 0, 4);
            tlpRaiz.SetColumnSpan(dgvMorosos, 2);
            tlpRaiz.Controls.Add(flpAcciones, 0, 5);
            tlpRaiz.SetColumnSpan(flpAcciones, 2);
            tlpRaiz.Controls.Add(lblAviso, 0, 6);
            tlpRaiz.SetColumnSpan(lblAviso, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 20);
            tlpRaiz.RowCount = 7;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblTituloCuotaAnio
            //
            lblTituloCuotaAnio.Anchor = AnchorStyles.Left;
            lblTituloCuotaAnio.AutoSize = true;
            lblTituloCuotaAnio.Name = "lblTituloCuotaAnio";
            lblTituloCuotaAnio.Text = "Cuota del año";
            //
            // btnVolver
            //
            btnVolver.Anchor = AnchorStyles.Right;
            btnVolver.Margin = new Padding(3, 3, 3, 6);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 38);
            btnVolver.TabIndex = 20;
            btnVolver.Text = "Volver";
            btnVolver.Click += btnVolver_Click;
            //
            // flpCuotaAnio
            //
            flpCuotaAnio.AutoSize = true;
            flpCuotaAnio.Controls.Add(lblAnio);
            flpCuotaAnio.Controls.Add(numAnio);
            flpCuotaAnio.Controls.Add(lblValor);
            flpCuotaAnio.Controls.Add(numValor);
            flpCuotaAnio.Controls.Add(btnGuardarValor);
            flpCuotaAnio.Controls.Add(btnGenerar);
            flpCuotaAnio.Dock = DockStyle.Fill;
            flpCuotaAnio.Name = "flpCuotaAnio";
            flpCuotaAnio.Tag = "NoEsDelFormulario";
            flpCuotaAnio.TabIndex = 0;
            flpCuotaAnio.WrapContents = false;
            //
            // lblAnio
            //
            lblAnio.AutoSize = true;
            lblAnio.Margin = new Padding(3, 10, 3, 0);
            lblAnio.Name = "lblAnio";
            lblAnio.Text = "Año:";
            //
            // numAnio
            //
            numAnio.Margin = new Padding(3, 6, 16, 3);
            numAnio.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
            numAnio.Minimum = new decimal(new int[] { 2000, 0, 0, 0 });
            numAnio.Name = "numAnio";
            numAnio.Size = new Size(90, 27);
            numAnio.TabIndex = 0;
            numAnio.Value = new decimal(new int[] { 2026, 0, 0, 0 });
            numAnio.ValueChanged += numAnio_ValueChanged;
            //
            // lblValor
            //
            lblValor.AutoSize = true;
            lblValor.Margin = new Padding(3, 10, 3, 0);
            lblValor.Name = "lblValor";
            lblValor.Text = "Valor de la cuota:";
            //
            // numValor
            //
            numValor.DecimalPlaces = 2;
            numValor.Margin = new Padding(3, 6, 16, 3);
            numValor.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numValor.Name = "numValor";
            numValor.Size = new Size(150, 27);
            numValor.TabIndex = 1;
            numValor.ThousandsSeparator = true;
            //
            // btnGuardarValor
            //
            btnGuardarValor.Margin = new Padding(3, 2, 8, 3);
            btnGuardarValor.Name = "btnGuardarValor";
            btnGuardarValor.Size = new Size(150, 40);
            btnGuardarValor.TabIndex = 2;
            btnGuardarValor.Text = "Guardar valor";
            btnGuardarValor.Click += btnGuardarValor_Click;
            //
            // btnGenerar
            //
            btnGenerar.Margin = new Padding(3, 2, 8, 3);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(380, 40);
            btnGenerar.TabIndex = 3;
            btnGenerar.Text = "Generar cuotas para todos los socios activos";
            btnGenerar.Click += btnGenerar_Click;
            //
            // lblDgvMorosos
            //
            lblDgvMorosos.AutoSize = true;
            lblDgvMorosos.Margin = new Padding(3, 22, 3, 3);
            lblDgvMorosos.Name = "lblDgvMorosos";
            lblDgvMorosos.Text = "Morosos";
            //
            // flpFiltros
            //
            flpFiltros.AutoSize = true;
            flpFiltros.Controls.Add(lblAnioMorosos);
            flpFiltros.Controls.Add(cmbAnioMorosos);
            flpFiltros.Controls.Add(lblBuscar);
            flpFiltros.Controls.Add(txtBuscar);
            flpFiltros.Controls.Add(chkDadosDeBaja);
            flpFiltros.Dock = DockStyle.Fill;
            flpFiltros.Name = "flpFiltros";
            flpFiltros.TabIndex = 1;
            flpFiltros.WrapContents = false;
            //
            // lblAnioMorosos
            //
            lblAnioMorosos.AutoSize = true;
            lblAnioMorosos.Margin = new Padding(3, 10, 3, 0);
            lblAnioMorosos.Name = "lblAnioMorosos";
            lblAnioMorosos.Text = "Cuotas de:";
            //
            // cmbAnioMorosos
            //
            cmbAnioMorosos.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAnioMorosos.Margin = new Padding(3, 6, 16, 3);
            cmbAnioMorosos.Name = "cmbAnioMorosos";
            cmbAnioMorosos.Size = new Size(170, 28);
            cmbAnioMorosos.TabIndex = 0;
            cmbAnioMorosos.SelectedIndexChanged += cmbAnioMorosos_SelectedIndexChanged;
            //
            // lblBuscar
            //
            lblBuscar.AutoSize = true;
            lblBuscar.Margin = new Padding(3, 10, 3, 0);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            txtBuscar.Margin = new Padding(3, 6, 16, 3);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Socio, DNI o responsable";
            txtBuscar.Size = new Size(280, 27);
            txtBuscar.TabIndex = 1;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // chkDadosDeBaja
            //
            chkDadosDeBaja.AutoSize = true;
            chkDadosDeBaja.Margin = new Padding(3, 9, 3, 3);
            chkDadosDeBaja.Name = "chkDadosDeBaja";
            chkDadosDeBaja.TabIndex = 2;
            chkDadosDeBaja.Text = "Incluir socios dados de baja";
            chkDadosDeBaja.CheckedChanged += chkDadosDeBaja_CheckedChanged;
            //
            // dgvMorosos
            //
            dgvMorosos.AllowUserToAddRows = false;
            dgvMorosos.AllowUserToDeleteRows = false;
            dgvMorosos.AllowUserToResizeRows = false;
            dgvMorosos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMorosos.Dock = DockStyle.Fill;
            dgvMorosos.MultiSelect = false;
            dgvMorosos.Name = "dgvMorosos";
            dgvMorosos.ReadOnly = true;
            dgvMorosos.RowHeadersVisible = false;
            dgvMorosos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMorosos.TabIndex = 2;
            dgvMorosos.CellDoubleClick += dgvMorosos_CellDoubleClick;
            dgvMorosos.DataBindingComplete += dgvMorosos_DataBindingComplete;
            //
            // flpAcciones
            //
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnCobrar);
            flpAcciones.Controls.Add(btnVerFicha);
            flpAcciones.Controls.Add(btnCopiarEmails);
            flpAcciones.Controls.Add(btnExportar);
            flpAcciones.Controls.Add(lblTotalMorosos);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.Margin = new Padding(0, 10, 0, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.TabIndex = 3;
            flpAcciones.WrapContents = false;
            //
            // btnCobrar
            //
            btnCobrar.Margin = new Padding(3, 0, 8, 0);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(160, 40);
            btnCobrar.TabIndex = 0;
            btnCobrar.Text = "Cobrar cuotas";
            btnCobrar.Click += btnCobrar_Click;
            //
            // btnVerFicha
            //
            btnVerFicha.Margin = new Padding(3, 0, 8, 0);
            btnVerFicha.Name = "btnVerFicha";
            btnVerFicha.Size = new Size(130, 40);
            btnVerFicha.TabIndex = 1;
            btnVerFicha.Text = "Ver ficha";
            btnVerFicha.Click += btnVerFicha_Click;
            //
            // btnCopiarEmails
            //
            btnCopiarEmails.Margin = new Padding(3, 0, 8, 0);
            btnCopiarEmails.Name = "btnCopiarEmails";
            btnCopiarEmails.Size = new Size(150, 40);
            btnCopiarEmails.TabIndex = 2;
            btnCopiarEmails.Text = "Copiar emails";
            btnCopiarEmails.Click += btnCopiarEmails_Click;
            //
            // btnExportar
            //
            btnExportar.Margin = new Padding(3, 0, 8, 0);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(160, 40);
            btnExportar.TabIndex = 3;
            btnExportar.Text = "Exportar a CSV";
            btnExportar.Click += btnExportar_Click;
            //
            // lblTotalMorosos
            //
            lblTotalMorosos.AutoSize = true;
            lblTotalMorosos.Margin = new Padding(16, 10, 3, 0);
            lblTotalMorosos.Name = "lblTotalMorosos";
            lblTotalMorosos.Text = "Total";
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(3, 8, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // FrmCuotas
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmCuotas";
            Text = "Cuotas y morosos";
            WindowState = FormWindowState.Maximized;
            Load += FrmCuotas_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpCuotaAnio.ResumeLayout(false);
            flpCuotaAnio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numAnio).EndInit();
            ((System.ComponentModel.ISupportInitialize)numValor).EndInit();
            flpFiltros.ResumeLayout(false);
            flpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMorosos).EndInit();
            flpAcciones.ResumeLayout(false);
            flpAcciones.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblTituloCuotaAnio;
        private Button btnVolver;
        private FlowLayoutPanel flpCuotaAnio;
        private Label lblAnio;
        private NumericUpDown numAnio;
        private Label lblValor;
        private NumericUpDown numValor;
        private Button btnGuardarValor;
        private Button btnGenerar;
        private Label lblDgvMorosos;
        private FlowLayoutPanel flpFiltros;
        private Label lblAnioMorosos;
        private ComboBox cmbAnioMorosos;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private CheckBox chkDadosDeBaja;
        private DataGridView dgvMorosos;
        private FlowLayoutPanel flpAcciones;
        private Button btnCobrar;
        private Button btnVerFicha;
        private Button btnCopiarEmails;
        private Button btnExportar;
        private Label lblTotalMorosos;
        private Label lblAviso;
    }
}
