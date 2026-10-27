namespace CooperadoraISFT94.Forms
{
    partial class FrmMovimientos
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
            lblDgvMovimientos = new Label();
            btnVolver = new Button();
            flpFiltros = new FlowLayoutPanel();
            lblCaja = new Label();
            cmbCaja = new ComboBox();
            lblTipo = new Label();
            cmbTipo = new ComboBox();
            lblMedio = new Label();
            cmbMedio = new ComboBox();
            lblGrupo = new Label();
            cmbGrupo = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvMovimientos = new DataGridView();
            flpAcciones = new FlowLayoutPanel();
            btnAnular = new Button();
            btnImprimir = new Button();
            btnExportar = new Button();
            btnExportarEgresos = new Button();
            lblAviso = new Label();
            tlpRaiz.SuspendLayout();
            flpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).BeginInit();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.Controls.Add(lblDgvMovimientos, 0, 0);
            tlpRaiz.Controls.Add(btnVolver, 1, 0);
            tlpRaiz.Controls.Add(flpFiltros, 0, 1);
            tlpRaiz.SetColumnSpan(flpFiltros, 2);
            tlpRaiz.Controls.Add(dgvMovimientos, 0, 2);
            tlpRaiz.SetColumnSpan(dgvMovimientos, 2);
            tlpRaiz.Controls.Add(flpAcciones, 0, 3);
            tlpRaiz.SetColumnSpan(flpAcciones, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 20);
            tlpRaiz.RowCount = 4;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblDgvMovimientos
            //
            lblDgvMovimientos.Anchor = AnchorStyles.Left;
            lblDgvMovimientos.AutoSize = true;
            lblDgvMovimientos.Name = "lblDgvMovimientos";
            lblDgvMovimientos.Text = "Movimientos de la caja";
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
            // flpFiltros
            //
            flpFiltros.AutoSize = true;
            flpFiltros.Controls.Add(lblCaja);
            flpFiltros.Controls.Add(cmbCaja);
            flpFiltros.Controls.Add(lblTipo);
            flpFiltros.Controls.Add(cmbTipo);
            flpFiltros.Controls.Add(lblMedio);
            flpFiltros.Controls.Add(cmbMedio);
            flpFiltros.Controls.Add(lblGrupo);
            flpFiltros.Controls.Add(cmbGrupo);
            flpFiltros.Controls.Add(lblBuscar);
            flpFiltros.Controls.Add(txtBuscar);
            flpFiltros.Dock = DockStyle.Fill;
            flpFiltros.Margin = new Padding(0, 6, 0, 6);
            flpFiltros.Name = "flpFiltros";
            flpFiltros.TabIndex = 0;
            //
            // lblCaja
            //
            lblCaja.AutoSize = true;
            lblCaja.Margin = new Padding(3, 10, 3, 0);
            lblCaja.Name = "lblCaja";
            lblCaja.Text = "Caja:";
            //
            // cmbCaja
            //
            cmbCaja.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCaja.Margin = new Padding(3, 6, 16, 3);
            cmbCaja.Name = "cmbCaja";
            cmbCaja.Size = new Size(320, 28);
            cmbCaja.TabIndex = 0;
            cmbCaja.SelectedIndexChanged += Filtro_Cambio;
            //
            // lblTipo
            //
            lblTipo.AutoSize = true;
            lblTipo.Margin = new Padding(3, 10, 3, 0);
            lblTipo.Name = "lblTipo";
            lblTipo.Text = "Tipo:";
            //
            // cmbTipo
            //
            cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipo.Margin = new Padding(3, 6, 16, 3);
            cmbTipo.Name = "cmbTipo";
            cmbTipo.Size = new Size(120, 28);
            cmbTipo.TabIndex = 1;
            cmbTipo.SelectedIndexChanged += Filtro_Cambio;
            //
            // lblMedio
            //
            lblMedio.AutoSize = true;
            lblMedio.Margin = new Padding(3, 10, 3, 0);
            lblMedio.Name = "lblMedio";
            lblMedio.Text = "Medio de pago:";
            //
            // cmbMedio
            //
            cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedio.Margin = new Padding(3, 6, 16, 3);
            cmbMedio.Name = "cmbMedio";
            cmbMedio.Size = new Size(150, 28);
            cmbMedio.TabIndex = 2;
            cmbMedio.SelectedIndexChanged += Filtro_Cambio;
            //
            // lblGrupo
            //
            lblGrupo.AutoSize = true;
            lblGrupo.Margin = new Padding(3, 10, 3, 0);
            lblGrupo.Name = "lblGrupo";
            lblGrupo.Text = "Grupo de egreso:";
            //
            // cmbGrupo
            //
            cmbGrupo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGrupo.Margin = new Padding(3, 6, 16, 3);
            cmbGrupo.Name = "cmbGrupo";
            cmbGrupo.Size = new Size(150, 28);
            cmbGrupo.TabIndex = 3;
            cmbGrupo.SelectedIndexChanged += Filtro_Cambio;
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
            txtBuscar.Margin = new Padding(3, 6, 3, 3);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Detalle, motivo o usuario";
            txtBuscar.Size = new Size(240, 27);
            txtBuscar.TabIndex = 4;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvMovimientos
            //
            dgvMovimientos.AllowUserToAddRows = false;
            dgvMovimientos.AllowUserToDeleteRows = false;
            dgvMovimientos.AllowUserToResizeRows = false;
            dgvMovimientos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMovimientos.Dock = DockStyle.Fill;
            dgvMovimientos.MultiSelect = false;
            dgvMovimientos.Name = "dgvMovimientos";
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.RowHeadersVisible = false;
            dgvMovimientos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMovimientos.TabIndex = 1;
            dgvMovimientos.DataBindingComplete += dgvMovimientos_DataBindingComplete;
            //
            // flpAcciones
            //
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnAnular);
            flpAcciones.Controls.Add(btnImprimir);
            flpAcciones.Controls.Add(btnExportar);
            flpAcciones.Controls.Add(btnExportarEgresos);
            flpAcciones.Controls.Add(lblAviso);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.Margin = new Padding(0, 10, 0, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.TabIndex = 2;
            flpAcciones.WrapContents = false;
            //
            // btnAnular
            //
            btnAnular.Margin = new Padding(3, 0, 8, 0);
            btnAnular.Name = "btnAnular";
            btnAnular.Size = new Size(190, 40);
            btnAnular.TabIndex = 0;
            btnAnular.Text = "Anular movimiento";
            btnAnular.Click += btnAnular_Click;
            //
            // btnImprimir
            //
            btnImprimir.Margin = new Padding(3, 0, 8, 0);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(230, 40);
            btnImprimir.TabIndex = 1;
            btnImprimir.Text = "Imprimir resumen de caja";
            btnImprimir.Click += btnImprimir_Click;
            //
            // btnExportar
            //
            btnExportar.Margin = new Padding(3, 0, 8, 0);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(160, 40);
            btnExportar.TabIndex = 2;
            btnExportar.Text = "Exportar a CSV";
            btnExportar.Click += btnExportar_Click;
            //
            // btnExportarEgresos
            //
            btnExportarEgresos.Margin = new Padding(3, 0, 8, 0);
            btnExportarEgresos.Name = "btnExportarEgresos";
            btnExportarEgresos.Size = new Size(250, 40);
            btnExportarEgresos.TabIndex = 3;
            btnExportarEgresos.Text = "Exportar egresos por grupo";
            btnExportarEgresos.Click += btnExportarEgresos_Click;
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(12, 10, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // FrmMovimientos
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmMovimientos";
            Text = "Movimientos";
            WindowState = FormWindowState.Maximized;
            Load += FrmMovimientos_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpFiltros.ResumeLayout(false);
            flpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMovimientos).EndInit();
            flpAcciones.ResumeLayout(false);
            flpAcciones.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblDgvMovimientos;
        private Button btnVolver;
        private FlowLayoutPanel flpFiltros;
        private Label lblCaja;
        private ComboBox cmbCaja;
        private Label lblTipo;
        private ComboBox cmbTipo;
        private Label lblMedio;
        private ComboBox cmbMedio;
        private Label lblGrupo;
        private ComboBox cmbGrupo;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvMovimientos;
        private FlowLayoutPanel flpAcciones;
        private Button btnAnular;
        private Button btnImprimir;
        private Button btnExportar;
        private Button btnExportarEgresos;
        private Label lblAviso;
    }
}
