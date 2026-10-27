namespace CooperadoraISFT94.Forms
{
    partial class FrmUsuarios
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
            lblNombreUsuario = new Label();
            txtNombreUsuario = new TextBox();
            lblNombre = new Label();
            txtNombre = new TextBox();
            lblApellido = new Label();
            txtApellido = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblRoles = new Label();
            lstRoles = new CheckedListBox();
            chkActivo = new CheckBox();
            tlpBotones = new TableLayoutPanel();
            btnGuardar = new Button();
            btnCancelarEdicion = new Button();
            btnEditar = new Button();
            btnEstado = new Button();
            lblAviso = new Label();
            tlpGrilla = new TableLayoutPanel();
            lblDgvUsuarios = new Label();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            dgvUsuarios = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).BeginInit();
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
            tlpFormulario.Controls.Add(lblModo, 0, 0);
            tlpFormulario.Controls.Add(lblNombreUsuario, 0, 1);
            tlpFormulario.Controls.Add(txtNombreUsuario, 0, 2);
            tlpFormulario.Controls.Add(lblNombre, 0, 3);
            tlpFormulario.Controls.Add(txtNombre, 0, 4);
            tlpFormulario.Controls.Add(lblApellido, 0, 5);
            tlpFormulario.Controls.Add(txtApellido, 0, 6);
            tlpFormulario.Controls.Add(lblPassword, 0, 7);
            tlpFormulario.Controls.Add(txtPassword, 0, 8);
            tlpFormulario.Controls.Add(lblRoles, 0, 9);
            tlpFormulario.Controls.Add(lstRoles, 0, 10);
            tlpFormulario.Controls.Add(chkActivo, 0, 11);
            tlpFormulario.Controls.Add(tlpBotones, 0, 12);
            tlpFormulario.Controls.Add(lblAviso, 0, 13);
            tlpFormulario.Dock = DockStyle.Fill;
            tlpFormulario.Margin = new Padding(0, 0, 40, 0);
            tlpFormulario.Name = "tlpFormulario";
            tlpFormulario.RowCount = 15;
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
            lblModo.Text = "Nuevo usuario";
            //
            // lblNombreUsuario
            //
            lblNombreUsuario.AutoSize = true;
            lblNombreUsuario.Margin = new Padding(3, 8, 3, 3);
            lblNombreUsuario.Name = "lblNombreUsuario";
            lblNombreUsuario.Text = "Nombre de usuario: *";
            //
            // txtNombreUsuario
            //
            txtNombreUsuario.Dock = DockStyle.Fill;
            txtNombreUsuario.Name = "txtNombreUsuario";
            txtNombreUsuario.PlaceholderText = "nombre.apellido";
            txtNombreUsuario.TabIndex = 1;
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
            txtNombre.TabIndex = 2;
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
            txtApellido.TabIndex = 3;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Margin = new Padding(3, 8, 3, 3);
            lblPassword.Name = "lblPassword";
            lblPassword.Text = "Contraseña: *";
            //
            // txtPassword
            //
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Name = "txtPassword";
            txtPassword.TabIndex = 4;
            txtPassword.UseSystemPasswordChar = true;
            //
            // lblRoles
            //
            lblRoles.AutoSize = true;
            lblRoles.Margin = new Padding(3, 8, 3, 3);
            lblRoles.Name = "lblRoles";
            lblRoles.Text = "Roles (uno o más): *";
            //
            // lstRoles
            //
            lstRoles.CheckOnClick = true;
            lstRoles.Dock = DockStyle.Fill;
            lstRoles.IntegralHeight = false;
            lstRoles.Size = new Size(200, 86);
            lstRoles.Name = "lstRoles";
            lstRoles.TabIndex = 5;
            //
            // chkActivo
            //
            chkActivo.AutoSize = true;
            chkActivo.Checked = true;
            chkActivo.Margin = new Padding(3, 10, 3, 3);
            chkActivo.Name = "chkActivo";
            chkActivo.TabIndex = 6;
            chkActivo.Text = "Usuario activo";
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
            tlpBotones.TabIndex = 7;
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
            btnEstado.Text = "Desactivar";
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
            tlpGrilla.Controls.Add(lblDgvUsuarios, 0, 0);
            tlpGrilla.Controls.Add(lblBuscar, 1, 0);
            tlpGrilla.Controls.Add(txtBuscar, 2, 0);
            tlpGrilla.Controls.Add(dgvUsuarios, 0, 1);
            tlpGrilla.SetColumnSpan(dgvUsuarios, 3);
            tlpGrilla.Dock = DockStyle.Fill;
            tlpGrilla.Name = "tlpGrilla";
            tlpGrilla.RowCount = 2;
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpGrilla.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpGrilla.TabIndex = 1;
            //
            // lblDgvUsuarios
            //
            lblDgvUsuarios.AutoSize = true;
            lblDgvUsuarios.Margin = new Padding(3, 12, 3, 3);
            lblDgvUsuarios.Name = "lblDgvUsuarios";
            lblDgvUsuarios.Text = "Usuarios";
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
            txtBuscar.PlaceholderText = "Nombre, usuario o rol";
            txtBuscar.TabIndex = 10;
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            //
            // dgvUsuarios
            //
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AllowUserToDeleteRows = false;
            dgvUsuarios.AllowUserToResizeRows = false;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.Dock = DockStyle.Fill;
            dgvUsuarios.MultiSelect = false;
            dgvUsuarios.Name = "dgvUsuarios";
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.TabIndex = 11;
            dgvUsuarios.CellDoubleClick += dgvUsuarios_CellDoubleClick;
            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
            dgvUsuarios.DataBindingComplete += dgvUsuarios_DataBindingComplete;
            //
            // FrmUsuarios
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(1000, 650);
            Name = "FrmUsuarios";
            Text = "Usuarios";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmUsuarios_FormClosing;
            Load += FrmUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)dgvUsuarios).EndInit();
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
        private Label lblNombreUsuario;
        private TextBox txtNombreUsuario;
        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblApellido;
        private TextBox txtApellido;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblRoles;
        private CheckedListBox lstRoles;
        private CheckBox chkActivo;
        private TableLayoutPanel tlpBotones;
        private Button btnGuardar;
        private Button btnCancelarEdicion;
        private Button btnEditar;
        private Button btnEstado;
        private Label lblAviso;
        private TableLayoutPanel tlpGrilla;
        private Label lblDgvUsuarios;
        private Label lblBuscar;
        private TextBox txtBuscar;
        private DataGridView dgvUsuarios;
    }
}
