namespace CooperadoraISFT94.Forms
{
    partial class FrmPrincipal
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
            pnlMenu = new Panel();
            btnMovimientos = new Button();
            btnEgresos = new Button();
            btnCaja = new Button();
            lblSeccionContador = new Label();
            btnIngresos = new Button();
            btnCuotas = new Button();
            btnSocios = new Button();
            lblSeccionOperador = new Label();
            btnAuditoria = new Button();
            btnCatalogos = new Button();
            btnUsuarios = new Button();
            lblSeccionAdministrador = new Label();
            btnInicio = new Button();
            lblUsuario = new Label();
            lblTitulo = new Label();
            btnMiPassword = new Button();
            btnCerrarSesion = new Button();
            btnSalir = new Button();
            pnlEncabezado = new Panel();
            lblPantalla = new Label();
            pnlContenido = new Panel();
            pnlMenu.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            SuspendLayout();
            //
            // pnlMenu
            //
            pnlMenu.Controls.Add(btnMovimientos);
            pnlMenu.Controls.Add(btnEgresos);
            pnlMenu.Controls.Add(btnCaja);
            pnlMenu.Controls.Add(lblSeccionContador);
            pnlMenu.Controls.Add(btnIngresos);
            pnlMenu.Controls.Add(btnCuotas);
            pnlMenu.Controls.Add(btnSocios);
            pnlMenu.Controls.Add(lblSeccionOperador);
            pnlMenu.Controls.Add(btnAuditoria);
            pnlMenu.Controls.Add(btnCatalogos);
            pnlMenu.Controls.Add(btnUsuarios);
            pnlMenu.Controls.Add(lblSeccionAdministrador);
            pnlMenu.Controls.Add(btnInicio);
            pnlMenu.Controls.Add(lblUsuario);
            pnlMenu.Controls.Add(lblTitulo);
            pnlMenu.Controls.Add(btnMiPassword);
            pnlMenu.Controls.Add(btnCerrarSesion);
            pnlMenu.Controls.Add(btnSalir);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Name = "pnlMenu";
            pnlMenu.AutoScroll = true;
            pnlMenu.Width = 260;
            pnlMenu.TabIndex = 0;
            //
            // lblTitulo
            //
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 56;
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Padding = new Padding(20, 0, 0, 0);
            lblTitulo.Text = "Cooperadora ISFT 194";
            lblTitulo.TextAlign = ContentAlignment.BottomLeft;
            //
            // lblUsuario
            //
            lblUsuario.Dock = DockStyle.Top;
            lblUsuario.Height = 74;
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Padding = new Padding(20, 4, 8, 0);
            lblUsuario.Text = "Usuario";
            //
            // btnInicio
            //
            btnInicio.Dock = DockStyle.Top;
            btnInicio.Height = 38;
            btnInicio.Name = "btnInicio";
            btnInicio.Padding = new Padding(16, 0, 0, 0);
            btnInicio.TabIndex = 0;
            btnInicio.Text = "Inicio";
            btnInicio.TextAlign = ContentAlignment.MiddleLeft;
            btnInicio.Click += btnInicio_Click;
            //
            // lblSeccionAdministrador
            //
            lblSeccionAdministrador.Dock = DockStyle.Top;
            lblSeccionAdministrador.Height = 30;
            lblSeccionAdministrador.Name = "lblSeccionAdministrador";
            lblSeccionAdministrador.Padding = new Padding(20, 0, 0, 2);
            lblSeccionAdministrador.Text = "ADMINISTRACIÓN";
            lblSeccionAdministrador.TextAlign = ContentAlignment.BottomLeft;
            //
            // btnUsuarios
            //
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.Height = 38;
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(16, 0, 0, 0);
            btnUsuarios.TabIndex = 1;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Click += btnUsuarios_Click;
            //
            // btnCatalogos
            //
            btnCatalogos.Dock = DockStyle.Top;
            btnCatalogos.Height = 38;
            btnCatalogos.Name = "btnCatalogos";
            btnCatalogos.Padding = new Padding(16, 0, 0, 0);
            btnCatalogos.TabIndex = 2;
            btnCatalogos.Text = "Catálogos";
            btnCatalogos.TextAlign = ContentAlignment.MiddleLeft;
            btnCatalogos.Click += btnCatalogos_Click;
            //
            // btnAuditoria
            //
            btnAuditoria.Dock = DockStyle.Top;
            btnAuditoria.Height = 38;
            btnAuditoria.Name = "btnAuditoria";
            btnAuditoria.Padding = new Padding(16, 0, 0, 0);
            btnAuditoria.TabIndex = 3;
            btnAuditoria.Text = "Auditoría";
            btnAuditoria.TextAlign = ContentAlignment.MiddleLeft;
            btnAuditoria.Click += btnAuditoria_Click;
            //
            // lblSeccionOperador
            //
            lblSeccionOperador.Dock = DockStyle.Top;
            lblSeccionOperador.Height = 30;
            lblSeccionOperador.Name = "lblSeccionOperador";
            lblSeccionOperador.Padding = new Padding(20, 0, 0, 2);
            lblSeccionOperador.Text = "SOCIOS Y COBROS";
            lblSeccionOperador.TextAlign = ContentAlignment.BottomLeft;
            //
            // btnSocios
            //
            btnSocios.Dock = DockStyle.Top;
            btnSocios.Height = 38;
            btnSocios.Name = "btnSocios";
            btnSocios.Padding = new Padding(16, 0, 0, 0);
            btnSocios.TabIndex = 4;
            btnSocios.Text = "Socios";
            btnSocios.TextAlign = ContentAlignment.MiddleLeft;
            btnSocios.Click += btnSocios_Click;
            //
            // btnCuotas
            //
            btnCuotas.Dock = DockStyle.Top;
            btnCuotas.Height = 38;
            btnCuotas.Name = "btnCuotas";
            btnCuotas.Padding = new Padding(16, 0, 0, 0);
            btnCuotas.TabIndex = 5;
            btnCuotas.Text = "Cuotas y morosos";
            btnCuotas.TextAlign = ContentAlignment.MiddleLeft;
            btnCuotas.Click += btnCuotas_Click;
            //
            // btnIngresos
            //
            btnIngresos.Dock = DockStyle.Top;
            btnIngresos.Height = 38;
            btnIngresos.Name = "btnIngresos";
            btnIngresos.Padding = new Padding(16, 0, 0, 0);
            btnIngresos.TabIndex = 6;
            btnIngresos.Text = "Donaciones y subsidios";
            btnIngresos.TextAlign = ContentAlignment.MiddleLeft;
            btnIngresos.Click += btnIngresos_Click;
            //
            // lblSeccionContador
            //
            lblSeccionContador.Dock = DockStyle.Top;
            lblSeccionContador.Height = 30;
            lblSeccionContador.Name = "lblSeccionContador";
            lblSeccionContador.Padding = new Padding(20, 0, 0, 2);
            lblSeccionContador.Text = "CAJA";
            lblSeccionContador.TextAlign = ContentAlignment.BottomLeft;
            //
            // btnCaja
            //
            btnCaja.Dock = DockStyle.Top;
            btnCaja.Height = 38;
            btnCaja.Name = "btnCaja";
            btnCaja.Padding = new Padding(16, 0, 0, 0);
            btnCaja.TabIndex = 7;
            btnCaja.Text = "Apertura y cierre";
            btnCaja.TextAlign = ContentAlignment.MiddleLeft;
            btnCaja.Click += btnCaja_Click;
            //
            // btnEgresos
            //
            btnEgresos.Dock = DockStyle.Top;
            btnEgresos.Height = 38;
            btnEgresos.Name = "btnEgresos";
            btnEgresos.Padding = new Padding(16, 0, 0, 0);
            btnEgresos.TabIndex = 8;
            btnEgresos.Text = "Egresos";
            btnEgresos.TextAlign = ContentAlignment.MiddleLeft;
            btnEgresos.Click += btnEgresos_Click;
            //
            // btnMovimientos
            //
            btnMovimientos.Dock = DockStyle.Top;
            btnMovimientos.Height = 38;
            btnMovimientos.Name = "btnMovimientos";
            btnMovimientos.Padding = new Padding(16, 0, 0, 0);
            btnMovimientos.TabIndex = 9;
            btnMovimientos.Text = "Movimientos";
            btnMovimientos.TextAlign = ContentAlignment.MiddleLeft;
            btnMovimientos.Click += btnMovimientos_Click;
            //
            // btnMiPassword
            //
            btnMiPassword.Dock = DockStyle.Bottom;
            btnMiPassword.Height = 38;
            btnMiPassword.Name = "btnMiPassword";
            btnMiPassword.Padding = new Padding(16, 0, 0, 0);
            btnMiPassword.TabIndex = 10;
            btnMiPassword.Text = "Mi contraseña";
            btnMiPassword.TextAlign = ContentAlignment.MiddleLeft;
            btnMiPassword.Click += btnMiPassword_Click;
            //
            // btnCerrarSesion
            //
            btnCerrarSesion.Dock = DockStyle.Bottom;
            btnCerrarSesion.Height = 38;
            btnCerrarSesion.Name = "btnCerrarSesion";
            btnCerrarSesion.Padding = new Padding(16, 0, 0, 0);
            btnCerrarSesion.TabIndex = 11;
            btnCerrarSesion.Text = "Cerrar sesión";
            btnCerrarSesion.TextAlign = ContentAlignment.MiddleLeft;
            btnCerrarSesion.Click += btnCerrarSesion_Click;
            //
            // btnSalir
            //
            btnSalir.Dock = DockStyle.Bottom;
            btnSalir.Height = 38;
            btnSalir.Name = "btnSalir";
            btnSalir.Padding = new Padding(16, 0, 0, 0);
            btnSalir.TabIndex = 12;
            btnSalir.Text = "Salir";
            btnSalir.TextAlign = ContentAlignment.MiddleLeft;
            btnSalir.Click += btnSalir_Click;
            //
            // pnlEncabezado
            //
            pnlEncabezado.Controls.Add(lblPantalla);
            pnlEncabezado.Dock = DockStyle.Top;
            pnlEncabezado.Height = 70;
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Padding = new Padding(40, 0, 40, 0);
            pnlEncabezado.TabIndex = 1;
            //
            // lblPantalla
            //
            lblPantalla.Dock = DockStyle.Fill;
            lblPantalla.Name = "lblPantalla";
            lblPantalla.Text = "Inicio";
            lblPantalla.TextAlign = ContentAlignment.BottomLeft;
            //
            // pnlContenido
            //
            pnlContenido.Dock = DockStyle.Fill;
            pnlContenido.Name = "pnlContenido";
            pnlContenido.TabIndex = 2;
            //
            // FrmPrincipal
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(pnlContenido);
            Controls.Add(pnlEncabezado);
            Controls.Add(pnlMenu);
            MinimumSize = new Size(1100, 680);
            Name = "FrmPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cooperadora ISFT 194";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmPrincipal_FormClosing;
            Load += FrmPrincipal_Load;
            pnlMenu.ResumeLayout(false);
            pnlEncabezado.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMenu;
        private Label lblTitulo;
        private Label lblUsuario;
        private Button btnInicio;
        private Label lblSeccionAdministrador;
        private Button btnUsuarios;
        private Button btnCatalogos;
        private Button btnAuditoria;
        private Label lblSeccionOperador;
        private Button btnSocios;
        private Button btnCuotas;
        private Button btnIngresos;
        private Label lblSeccionContador;
        private Button btnCaja;
        private Button btnEgresos;
        private Button btnMovimientos;
        private Button btnMiPassword;
        private Button btnCerrarSesion;
        private Button btnSalir;
        private Panel pnlEncabezado;
        private Label lblPantalla;
        private Panel pnlContenido;
    }
}
