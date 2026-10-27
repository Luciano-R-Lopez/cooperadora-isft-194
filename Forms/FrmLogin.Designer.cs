namespace CooperadoraISFT94.Forms
{
    partial class FrmLogin
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
            pnlTarjetaLogin = new Panel();
            tlpTarjeta = new TableLayoutPanel();
            lblTituloInstitucion = new Label();
            lblSubtitulo = new Label();
            lblUsuario = new Label();
            txtUsuario = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnIngresar = new Button();
            btnSalir = new Button();
            lblAyuda = new Label();
            tlpRaiz.SuspendLayout();
            pnlTarjetaLogin.SuspendLayout();
            tlpTarjeta.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 3;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpRaiz.Controls.Add(pnlTarjetaLogin, 1, 1);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.RowCount = 3;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpRaiz.TabIndex = 0;
            //
            // pnlTarjetaLogin
            //
            pnlTarjetaLogin.AutoSize = true;
            pnlTarjetaLogin.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pnlTarjetaLogin.Controls.Add(tlpTarjeta);
            pnlTarjetaLogin.Name = "pnlTarjetaLogin";
            pnlTarjetaLogin.Padding = new Padding(36, 30, 36, 30);
            pnlTarjetaLogin.TabIndex = 0;
            //
            // tlpTarjeta
            //
            tlpTarjeta.AutoSize = true;
            tlpTarjeta.ColumnCount = 2;
            tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tlpTarjeta.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tlpTarjeta.Controls.Add(lblTituloInstitucion, 0, 0);
            tlpTarjeta.SetColumnSpan(lblTituloInstitucion, 2);
            tlpTarjeta.Controls.Add(lblSubtitulo, 0, 1);
            tlpTarjeta.SetColumnSpan(lblSubtitulo, 2);
            tlpTarjeta.Controls.Add(lblUsuario, 0, 2);
            tlpTarjeta.SetColumnSpan(lblUsuario, 2);
            tlpTarjeta.Controls.Add(txtUsuario, 0, 3);
            tlpTarjeta.SetColumnSpan(txtUsuario, 2);
            tlpTarjeta.Controls.Add(lblPassword, 0, 4);
            tlpTarjeta.SetColumnSpan(lblPassword, 2);
            tlpTarjeta.Controls.Add(txtPassword, 0, 5);
            tlpTarjeta.SetColumnSpan(txtPassword, 2);
            tlpTarjeta.Controls.Add(btnIngresar, 0, 6);
            tlpTarjeta.Controls.Add(btnSalir, 1, 6);
            tlpTarjeta.Controls.Add(lblAyuda, 0, 7);
            tlpTarjeta.SetColumnSpan(lblAyuda, 2);
            tlpTarjeta.Location = new Point(36, 30);
            tlpTarjeta.Name = "tlpTarjeta";
            tlpTarjeta.RowCount = 8;
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpTarjeta.TabIndex = 0;
            //
            // lblTituloInstitucion
            //
            lblTituloInstitucion.AutoSize = true;
            lblTituloInstitucion.Margin = new Padding(3, 0, 3, 4);
            lblTituloInstitucion.Name = "lblTituloInstitucion";
            lblTituloInstitucion.TabIndex = 0;
            lblTituloInstitucion.Text = "Cooperadora ISFT 194";
            //
            // lblSubtitulo
            //
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Margin = new Padding(3, 0, 3, 18);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Ingrese con su usuario y contraseña";
            //
            // lblUsuario
            //
            lblUsuario.AutoSize = true;
            lblUsuario.Margin = new Padding(3, 8, 3, 3);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario: *";
            //
            // txtUsuario
            //
            txtUsuario.Dock = DockStyle.Fill;
            txtUsuario.Name = "txtUsuario";
            txtUsuario.TabIndex = 0;
            //
            // lblPassword
            //
            lblPassword.AutoSize = true;
            lblPassword.Margin = new Padding(3, 8, 3, 3);
            lblPassword.Name = "lblPassword";
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Contraseña: *";
            //
            // txtPassword
            //
            txtPassword.Dock = DockStyle.Fill;
            txtPassword.Name = "txtPassword";
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            //
            // btnIngresar
            //
            btnIngresar.Dock = DockStyle.Fill;
            btnIngresar.Margin = new Padding(3, 22, 6, 3);
            btnIngresar.Name = "btnIngresar";
            btnIngresar.Size = new Size(170, 40);
            btnIngresar.TabIndex = 2;
            btnIngresar.Text = "Ingresar";
            btnIngresar.Click += btnIngresar_Click;
            //
            // btnSalir
            //
            btnSalir.DialogResult = DialogResult.Cancel;
            btnSalir.Dock = DockStyle.Fill;
            btnSalir.Margin = new Padding(6, 22, 3, 3);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(170, 40);
            btnSalir.TabIndex = 3;
            btnSalir.Text = "Salir";
            //
            // lblAyuda
            //
            lblAyuda.AutoSize = true;
            lblAyuda.Margin = new Padding(3, 16, 3, 0);
            lblAyuda.MaximumSize = new Size(360, 0);
            lblAyuda.Name = "lblAyuda";
            lblAyuda.TabIndex = 4;
            lblAyuda.Text = "Si olvidó su contraseña, pídale a un administrador que le asigne una nueva.";
            //
            // FrmLogin
            //
            AcceptButton = btnIngresar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnSalir;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(800, 600);
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cooperadora ISFT 194 - Iniciar sesión";
            WindowState = FormWindowState.Maximized;
            Shown += FrmLogin_Shown;
            tlpTarjeta.ResumeLayout(false);
            tlpTarjeta.PerformLayout();
            pnlTarjetaLogin.ResumeLayout(false);
            pnlTarjetaLogin.PerformLayout();
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Panel pnlTarjetaLogin;
        private TableLayoutPanel tlpTarjeta;
        private Label lblTituloInstitucion;
        private Label lblSubtitulo;
        private Label lblUsuario;
        private TextBox txtUsuario;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnIngresar;
        private Button btnSalir;
        private Label lblAyuda;
    }
}
