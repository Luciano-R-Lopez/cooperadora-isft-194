namespace CooperadoraISFT94.Forms
{
    partial class FrmSocios
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
            lblModo = new Label();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblDni = new Label();
            txtDni = new TextBox();
            lblTelefono = new Label();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblCategoria = new Label();
            cmbCategoria = new ComboBox();
            lblResponsable = new Label();
            cmbResponsable = new ComboBox();
            tlpBotones = new TableLayoutPanel();
            btnGuardar = new Button();
            btnCancelarEdicion = new Button();
            btnEditar = new Button();
            btnEstado = new Button();
            lblAviso = new Label();
            tlpGrilla = new TableLayoutPanel();
            lblDgvSocios = new Label();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvSocios = new DataGridView();
            flpAcciones = new FlowLayoutPanel();
            btnVerFicha = new Button();
            btnExportar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvSocios).BeginInit();
            tlpRaiz.SuspendLayout();
            tlpFormulario.SuspendLayout();
            tlpBotones.SuspendLayout();
            tlpGrilla.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
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
            btnVolver.TabIndex = 30;
            btnVolver.Text = "Volver";
            btnVolver.Click += btnVolver_Click;
            //
            // tlpFormulario
            //
            tlpFormulario.AutoScroll = true;
            tlpFormulario.ColumnCount = 1;
            tlpFormulario.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFormulario.Controls.Add(lblModo, 0, 0);
            tlpFormulario.Controls.Add(lblNombre, 0, 1);
            tlpFormulario.Controls.Add(txtNombre, 0, 2);
            tlpFormulario.Controls.Add(lblApellido, 0, 3);
            tlpFormulario.Controls.Add(txtApellido, 0, 4);
            tlpFormulario.Controls.Add(lblDni, 0, 5);
            tlpFormulario.Controls.Add(txtDni, 0, 6);
            tlpFormulario.Controls.Add(lblTelefono, 0, 7);
            tlpFormulario.Controls.Add(txtTelefono, 0, 8);
            tlpFormulario.Controls.Add(lblEmail, 0, 9);
            tlpFormulario.Controls.Add(txtEmail, 0, 10);
            tlpFormulario.Controls.Add(lblCategoria, 0, 11);
            tlpFormulario.Controls.Add(cmbCategoria, 0, 12);
            tlpFormulario.Controls.Add(lblResponsable, 0, 13);
            tlpFormulario.Controls.Add(cmbResponsable, 0, 14);
            tlpFormulario.Controls.Add(tlpBotones, 0, 15);
            tlpFormulario.Controls.Add(lblAviso, 0, 16);
            tlpFormulario.Dock = DockStyle.Fill;
            tlpFormulario.Margin = new Padding(0, 0, 40, 0);
            tlpFormulario.Name = "tlpFormulario";
            tlpFormulario.RowCount = 18;
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
            // lblModo
            //
            lblModo.AutoSize = true;
            lblModo.Margin = new Padding(3, 0, 3, 2);
            lblModo.Name = "lblModo";
            lblModo.Text = "Nuevo socio";
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
            // lblApellido
            //
            lblApellido.AutoSize = true;
            lblApellido.Margin = new Padding(3, 8, 3, 3);
            lblApellido.Name = "lblApellido";
            lblApellido.Text = "Apellido: *";
            //
            // txtApellido
            //
            txtApellido.Dock = DockStyle.Fill;
            txtApellido.Name = "txtApellido";
            txtApellido.TabIndex = 2;
            //
            // lblDni
            //
            lblDni.AutoSize = true;
            lblDni.Margin = new Padding(3, 8, 3, 3);
            lblDni.Name = "lblDni";
            lblDni.Text = "DNI: *";
            //
            // txtDni
            //
            txtDni.Dock = DockStyle.Fill;
            txtDni.Name = "txtDni";
            txtDni.PlaceholderText = "30123456";
            txtDni.TabIndex = 3;
            //
            // lblTelefono
            //
            lblTelefono.AutoSize = true;
            lblTelefono.Margin = new Padding(3, 8, 3, 3);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Text = "Teléfono:";
            //
            // txtTelefono
            //
            txtTelefono.Dock = DockStyle.Fill;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.PlaceholderText = "11 5555-0101";
            txtTelefono.TabIndex = 4;
            //
            // lblEmail
            //
            lblEmail.AutoSize = true;
            lblEmail.Margin = new Padding(3, 8, 3, 3);
            lblEmail.Name = "lblEmail";
            lblEmail.Text = "Email:";
            //
            // txtEmail
            //
            txtEmail.Dock = DockStyle.Fill;
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "nombre@ejemplo.com";
            txtEmail.TabIndex = 5;
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
            cmbCategoria.TabIndex = 6;
            cmbCategoria.SelectedIndexChanged += cmbCategoria_SelectedIndexChanged;
            //
            // lblResponsable
            //
            lblResponsable.AutoSize = true;
            lblResponsable.Margin = new Padding(3, 8, 3, 3);
            lblResponsable.Name = "lblResponsable";
            lblResponsable.Text = "Responsable (quien paga por el socio):";
            //
            // cmbResponsable
            //
            cmbResponsable.Dock = DockStyle.Fill;
            cmbResponsable.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbResponsable.Name = "cmbResponsable";
            cmbResponsable.TabIndex = 7;
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
            tlpBotones.TabIndex = 8;
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
            btnEditar.Text = "Editar";
            btnEditar.Click += btnEditar_Click;
            //
            // btnEstado
            //
            btnEstado.Dock = DockStyle.Fill;
            btnEstado.Margin = new Padding(8, 0, 0, 0);
            btnEstado.Name = "btnEstado";
            btnEstado.Size = new Size(94, 40);
            btnEstado.TabIndex = 3;
            btnEstado.Text = "Dar de baja";
            btnEstado.Click += btnEstado_Click;
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(3, 10, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // tlpGrilla
            //
            tlpGrilla.ColumnCount = 3;
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpGrilla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            tlpGrilla.Controls.Add(lblDgvSocios, 0, 0);
            tlpGrilla.Controls.Add(lblBuscar, 1, 0);
            tlpGrilla.Controls.Add(txtBuscar, 2, 0);
            tlpGrilla.Controls.Add(dgvSocios, 0, 1);
            tlpGrilla.SetColumnSpan(dgvSocios, 3);
            tlpGrilla.Controls.Add(flpAcciones, 0, 2);
            tlpGrilla.SetColumnSpan(flpAcciones, 3);
            tlpGrilla.Dock = DockStyle.Fill;
            tlpGrilla.Name = "tlpGrilla";
            tlpGrilla.RowCount = 3;
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpGrilla.TabIndex = 1;
            //
            // lblDgvSocios
            //
            lblDgvSocios.AutoSize = true;
            lblDgvSocios.Margin = new Padding(3, 12, 3, 3);
            lblDgvSocios.Name = "lblDgvSocios";
            lblDgvSocios.Text = "Socios";
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
            txtBuscar.PlaceholderText = "Nombre, DNI, responsable...";
            txtBuscar.TabIndex = 10;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvSocios
            //
            dgvSocios.AllowUserToAddRows = false;
            dgvSocios.AllowUserToDeleteRows = false;
            dgvSocios.AllowUserToResizeRows = false;
            dgvSocios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSocios.Dock = DockStyle.Fill;
            dgvSocios.MultiSelect = false;
            dgvSocios.Name = "dgvSocios";
            dgvSocios.ReadOnly = true;
            dgvSocios.RowHeadersVisible = false;
            dgvSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSocios.TabIndex = 11;
            dgvSocios.CellDoubleClick += dgvSocios_CellDoubleClick;
            dgvSocios.SelectionChanged += dgvSocios_SelectionChanged;
            dgvSocios.DataBindingComplete += dgvSocios_DataBindingComplete;
            //
            // flpAcciones
            //
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnVerFicha);
            flpAcciones.Controls.Add(btnExportar);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.Margin = new Padding(0, 10, 0, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.TabIndex = 12;
            //
            // btnVerFicha
            //
            btnVerFicha.Margin = new Padding(3, 0, 8, 0);
            btnVerFicha.Name = "btnVerFicha";
            btnVerFicha.Size = new Size(230, 40);
            btnVerFicha.TabIndex = 0;
            btnVerFicha.Text = "Ver ficha y cuotas del socio";
            btnVerFicha.Click += btnVerFicha_Click;
            //
            // btnExportar
            //
            btnExportar.Margin = new Padding(3, 0, 8, 0);
            btnExportar.Name = "btnExportar";
            btnExportar.Size = new Size(160, 40);
            btnExportar.TabIndex = 1;
            btnExportar.Text = "Exportar a CSV";
            btnExportar.Click += btnExportar_Click;
            //
            // FrmSocios
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmSocios";
            Text = "Socios";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmSocios_FormClosing;
            Load += FrmSocios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvSocios).EndInit();
            flpAcciones.ResumeLayout(false);
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
        private Label lblModo;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblDni;
        private TextBox txtDni;
        private Label lblTelefono;
        private TextBox txtTelefono;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblCategoria;
        private ComboBox cmbCategoria;
        private Label lblResponsable;
        private ComboBox cmbResponsable;
        private TableLayoutPanel tlpBotones;
        private Button btnGuardar;
        private Button btnCancelarEdicion;
        private Button btnEditar;
        private Button btnEstado;
        private Label lblAviso;
        private TableLayoutPanel tlpGrilla;
        private Label lblDgvSocios;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvSocios;
        private FlowLayoutPanel flpAcciones;
        private Button btnVerFicha;
        private Button btnExportar;
    }
}
