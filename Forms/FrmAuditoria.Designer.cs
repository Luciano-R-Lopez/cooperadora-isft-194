namespace CooperadoraISFT94.Forms
{
    partial class FrmAuditoria
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
            lblDgvAuditoria = new Label();
            btnVolver = new Button();
            flpFiltros = new FlowLayoutPanel();
            chkDesde = new CheckBox();
            dtpDesde = new Utils.CampoFecha();
            chkHasta = new CheckBox();
            dtpHasta = new Utils.CampoFecha();
            lblAccion = new Label();
            cmbAccion = new ComboBox();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvAuditoria = new DataGridView();
            flpAcciones = new FlowLayoutPanel();
            btnExportar = new Button();
            tlpRaiz.SuspendLayout();
            flpFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).BeginInit();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.Controls.Add(lblDgvAuditoria, 0, 0);
            tlpRaiz.Controls.Add(btnVolver, 1, 0);
            tlpRaiz.Controls.Add(flpFiltros, 0, 1);
            tlpRaiz.SetColumnSpan(flpFiltros, 2);
            tlpRaiz.Controls.Add(dgvAuditoria, 0, 2);
            tlpRaiz.SetColumnSpan(dgvAuditoria, 2);
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
            // lblDgvAuditoria
            //
            lblDgvAuditoria.Anchor = AnchorStyles.Left;
            lblDgvAuditoria.AutoSize = true;
            lblDgvAuditoria.Name = "lblDgvAuditoria";
            lblDgvAuditoria.Text = "Quién hizo qué y cuándo";
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
            flpFiltros.Controls.Add(chkDesde);
            flpFiltros.Controls.Add(dtpDesde);
            flpFiltros.Controls.Add(chkHasta);
            flpFiltros.Controls.Add(dtpHasta);
            flpFiltros.Controls.Add(lblAccion);
            flpFiltros.Controls.Add(cmbAccion);
            flpFiltros.Controls.Add(lblBuscar);
            flpFiltros.Controls.Add(txtBuscar);
            flpFiltros.Dock = DockStyle.Fill;
            flpFiltros.Margin = new Padding(0, 6, 0, 6);
            flpFiltros.Name = "flpFiltros";
            flpFiltros.TabIndex = 0;
            //
            // chkDesde
            //
            chkDesde.AutoSize = true;
            chkDesde.Margin = new Padding(3, 9, 3, 3);
            chkDesde.Name = "chkDesde";
            chkDesde.TabIndex = 0;
            chkDesde.Text = "Desde";
            chkDesde.CheckedChanged += chkDesde_CheckedChanged;
            //
            // dtpDesde
            //
            dtpDesde.Enabled = false;
            dtpDesde.Margin = new Padding(3, 6, 16, 3);
            dtpDesde.Name = "dtpDesde";
            dtpDesde.Size = new Size(140, 27);
            dtpDesde.TabIndex = 1;
            dtpDesde.ValueChanged += Filtro_Cambio;
            //
            // chkHasta
            //
            chkHasta.AutoSize = true;
            chkHasta.Margin = new Padding(3, 9, 3, 3);
            chkHasta.Name = "chkHasta";
            chkHasta.TabIndex = 2;
            chkHasta.Text = "Hasta";
            chkHasta.CheckedChanged += chkHasta_CheckedChanged;
            //
            // dtpHasta
            //
            dtpHasta.Enabled = false;
            dtpHasta.Margin = new Padding(3, 6, 16, 3);
            dtpHasta.Name = "dtpHasta";
            dtpHasta.Size = new Size(140, 27);
            dtpHasta.TabIndex = 3;
            dtpHasta.ValueChanged += Filtro_Cambio;
            //
            // lblAccion
            //
            lblAccion.AutoSize = true;
            lblAccion.Margin = new Padding(3, 10, 3, 0);
            lblAccion.Name = "lblAccion";
            lblAccion.Text = "Acción:";
            //
            // cmbAccion
            //
            cmbAccion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAccion.Margin = new Padding(3, 6, 16, 3);
            cmbAccion.Name = "cmbAccion";
            cmbAccion.Size = new Size(150, 28);
            cmbAccion.TabIndex = 4;
            cmbAccion.SelectedIndexChanged += Filtro_Cambio;
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
            txtBuscar.PlaceholderText = "Usuario, dato o detalle";
            txtBuscar.Size = new Size(260, 27);
            txtBuscar.TabIndex = 5;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvAuditoria
            //
            dgvAuditoria.AllowUserToAddRows = false;
            dgvAuditoria.AllowUserToDeleteRows = false;
            dgvAuditoria.AllowUserToResizeRows = false;
            dgvAuditoria.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAuditoria.Dock = DockStyle.Fill;
            dgvAuditoria.MultiSelect = false;
            dgvAuditoria.Name = "dgvAuditoria";
            dgvAuditoria.ReadOnly = true;
            dgvAuditoria.RowHeadersVisible = false;
            dgvAuditoria.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAuditoria.TabIndex = 1;
            //
            // flpAcciones
            //
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnExportar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.Margin = new Padding(0, 10, 0, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.TabIndex = 2;
            //
            // btnExportar
            //
            btnExportar.Margin = new Padding(3, 0, 8, 0);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(160, 40);
            btnExportar.TabIndex = 0;
            btnExportar.Text = "Exportar a CSV";
            btnExportar.Click += btnExportar_Click;
            //
            // FrmAuditoria
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmAuditoria";
            Text = "Auditoría";
            WindowState = FormWindowState.Maximized;
            Load += FrmAuditoria_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpFiltros.ResumeLayout(false);
            flpFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuditoria).EndInit();
            flpAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblDgvAuditoria;
        private Button btnVolver;
        private FlowLayoutPanel flpFiltros;
        private CheckBox chkDesde;
        private Utils.CampoFecha dtpDesde;
        private CheckBox chkHasta;
        private Utils.CampoFecha dtpHasta;
        private Label lblAccion;
        private ComboBox cmbAccion;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvAuditoria;
        private FlowLayoutPanel flpAcciones;
        private Button btnExportar;
    }
}
