namespace CooperadoraISFT94.Forms
{
    partial class FrmCatalogos
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
            btnVolver = new Button();
            tlpFormulario = new TableLayoutPanel();
            lblCatalogo = new Label();
            cmbCatalogo = new ComboBox();
            lblModo = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            tlpBotones = new TableLayoutPanel();
            btnGuardar = new Button();
            btnCancelarEdicion = new Button();
            btnEditar = new Button();
            btnEstado = new Button();
            lblAviso = new Label();
            lblAyuda = new Label();
            tlpGrilla = new TableLayoutPanel();
            lblDgvItems = new Label();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvItems = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            tlpRaiz.SuspendLayout();
            tlpFormulario.SuspendLayout();
            tlpBotones.SuspendLayout();
            tlpGrilla.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            tlpRaiz.Controls.Add(btnVolver, 0, 0);
            tlpRaiz.SetColumnSpan(btnVolver, 2);
            tlpRaiz.Controls.Add(tlpFormulario, 0, 1);
            tlpRaiz.Controls.Add(tlpGrilla, 1, 1);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 30);
            tlpRaiz.RowCount = 2;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // btnVolver
            //
            btnVolver.Anchor = AnchorStyles.Right;
            btnVolver.Margin = new Padding(3, 3, 3, 10);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 38);
            btnVolver.TabIndex = 20;
            btnVolver.Text = "Volver";
            btnVolver.Click += btnVolver_Click;
            //
            // tlpFormulario
            //
            tlpFormulario.ColumnCount = 1;
            tlpFormulario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFormulario.Controls.Add(lblCatalogo, 0, 0);
            tlpFormulario.Controls.Add(cmbCatalogo, 0, 1);
            tlpFormulario.Controls.Add(lblModo, 0, 2);
            tlpFormulario.Controls.Add(lblNombre, 0, 3);
            tlpFormulario.Controls.Add(txtNombre, 0, 4);
            tlpFormulario.Controls.Add(tlpBotones, 0, 5);
            tlpFormulario.Controls.Add(lblAviso, 0, 6);
            tlpFormulario.Controls.Add(lblAyuda, 0, 7);
            tlpFormulario.Dock = DockStyle.Fill;
            tlpFormulario.Margin = new Padding(0, 0, 40, 0);
            tlpFormulario.Name = "tlpFormulario";
            tlpFormulario.RowCount = 9;
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpFormulario.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFormulario.TabIndex = 0;
            //
            // lblCatalogo
            //
            lblCatalogo.AutoSize = true;
            lblCatalogo.Margin = new Padding(3, 0, 3, 3);
            lblCatalogo.Name = "lblCatalogo";
            lblCatalogo.Text = "Catálogo:";
            //
            // cmbCatalogo
            //
            cmbCatalogo.Dock = DockStyle.Fill;
            cmbCatalogo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCatalogo.Margin = new Padding(3, 3, 3, 20);
            cmbCatalogo.Name = "cmbCatalogo";
            cmbCatalogo.Tag = "NoEsDelFormulario";
            cmbCatalogo.TabIndex = 0;
            cmbCatalogo.SelectedIndexChanged += cmbCatalogo_SelectedIndexChanged;
            //
            // lblModo
            //
            lblModo.AutoSize = true;
            lblModo.Margin = new Padding(3, 0, 3, 2);
            lblModo.Name = "lblModo";
            lblModo.Text = "Nuevo ítem";
            //
            // lblNombre
            //
            lblNombre.AutoSize = true;
            lblNombre.Margin = new Padding(3, 8, 3, 3);
            lblNombre.Name = "lblNombre";
            lblNombre.Text = "Nombre: *";
            //
            // txtNombre
            //
            txtNombre.Dock = DockStyle.Fill;
            txtNombre.Name = "txtNombre";
            txtNombre.TabIndex = 1;
            //
            // tlpBotones
            //
            tlpBotones.AutoSize = true;
            tlpBotones.ColumnCount = 2;
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBotones.Controls.Add(btnGuardar, 0, 0);
            tlpBotones.SetColumnSpan(btnGuardar, 2);
            tlpBotones.Controls.Add(btnCancelarEdicion, 1, 0);
            tlpBotones.Controls.Add(btnEditar, 0, 1);
            tlpBotones.Controls.Add(btnEstado, 1, 1);
            tlpBotones.Dock = DockStyle.Top;
            tlpBotones.Margin = new Padding(3, 16, 3, 3);
            tlpBotones.Name = "tlpBotones";
            tlpBotones.RowCount = 2;
            tlpBotones.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpBotones.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpBotones.TabIndex = 2;
            //
            // btnGuardar
            //
            btnGuardar.Dock = DockStyle.Fill;
            btnGuardar.Margin = new Padding(0, 0, 0, 8);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(94, 40);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            //
            // btnCancelarEdicion
            //
            btnCancelarEdicion.Dock = DockStyle.Fill;
            btnCancelarEdicion.Margin = new Padding(8, 0, 0, 8);
            btnCancelarEdicion.Name = "btnCancelarEdicion";
            btnCancelarEdicion.Size = new Size(94, 40);
            btnCancelarEdicion.TabIndex = 1;
            btnCancelarEdicion.Text = "Cancelar";
            btnCancelarEdicion.Visible = false;
            btnCancelarEdicion.Click += btnCancelarEdicion_Click;
            //
            // btnEditar
            //
            btnEditar.Dock = DockStyle.Fill;
            btnEditar.Margin = new Padding(0);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(94, 40);
            btnEditar.TabIndex = 2;
            btnEditar.Text = "Renombrar";
            btnEditar.Click += btnEditar_Click;
            //
            // btnEstado
            //
            btnEstado.Dock = DockStyle.Fill;
            btnEstado.Margin = new Padding(8, 0, 0, 0);
            btnEstado.Name = "btnEstado";
            btnEstado.Size = new Size(94, 40);
            btnEstado.TabIndex = 3;
            btnEstado.Text = "Desactivar";
            btnEstado.Click += btnEstado_Click;
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
            lblAyuda.MaximumSize = new Size(380, 0);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.Text = "Un ítem desactivado no se ofrece para movimientos nuevos; los ya registrados no cambian.";
            //
            // tlpGrilla
            //
            tlpGrilla.ColumnCount = 3;
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            tlpGrilla.Controls.Add(lblDgvItems, 0, 0);
            tlpGrilla.Controls.Add(lblBuscar, 1, 0);
            tlpGrilla.Controls.Add(txtBuscar, 2, 0);
            tlpGrilla.Controls.Add(dgvItems, 0, 1);
            tlpGrilla.SetColumnSpan(dgvItems, 3);
            tlpGrilla.Dock = DockStyle.Fill;
            tlpGrilla.Name = "tlpGrilla";
            tlpGrilla.RowCount = 2;
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpGrilla.TabIndex = 1;
            //
            // lblDgvItems
            //
            lblDgvItems.AutoSize = true;
            lblDgvItems.Margin = new Padding(3, 12, 3, 3);
            lblDgvItems.Name = "lblDgvItems";
            lblDgvItems.Text = "Ítems";
            //
            // lblBuscar
            //
            lblBuscar.Anchor = AnchorStyles.Right;
            lblBuscar.AutoSize = true;
            lblBuscar.Margin = new Padding(3, 12, 6, 3);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Text = "Buscar:";
            //
            // txtBuscar
            //
            txtBuscar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtBuscar.Margin = new Padding(3, 12, 3, 3);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Buscar...";
            txtBuscar.TabIndex = 10;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvItems
            //
            dgvItems.AllowUserToAddRows = false;
            dgvItems.AllowUserToDeleteRows = false;
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.MultiSelect = false;
            dgvItems.Name = "dgvItems";
            dgvItems.ReadOnly = true;
            dgvItems.RowHeadersVisible = false;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.TabIndex = 11;
            dgvItems.CellDoubleClick += dgvItems_CellDoubleClick;
            dgvItems.SelectionChanged += dgvItems_SelectionChanged;
            dgvItems.DataBindingComplete += dgvItems_DataBindingComplete;
            //
            // FrmCatalogos
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmCatalogos";
            Text = "Catálogos";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmCatalogos_FormClosing;
            Load += FrmCatalogos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            tlpGrilla.ResumeLayout(false);
            tlpGrilla.PerformLayout();
            tlpBotones.ResumeLayout(false);
            tlpFormulario.ResumeLayout(false);
            tlpFormulario.PerformLayout();
            tlpRaiz.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Button btnVolver;
        private TableLayoutPanel tlpFormulario;
        private Label lblCatalogo;
        private ComboBox cmbCatalogo;
        private Label lblModo;
        private Label lblNombre;
        private TextBox txtNombre;
        private TableLayoutPanel tlpBotones;
        private Button btnGuardar;
        private Button btnCancelarEdicion;
        private Button btnEditar;
        private Button btnEstado;
        private Label lblAviso;
        private Label lblAyuda;
        private TableLayoutPanel tlpGrilla;
        private Label lblDgvItems;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvItems;
    }
}
