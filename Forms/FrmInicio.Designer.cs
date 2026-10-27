namespace CooperadoraISFT94.Forms
{
    partial class FrmInicio
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
            lblSaludo = new Label();
            lblDatoFecha = new Label();
            lblAlerta = new Label();
            flpTarjetas = new FlowLayoutPanel();
            pnlTarjetaCaja = new Panel();
            lblResumenDetalleCaja = new Label();
            lblResumenValorCaja = new Label();
            lblResumenTituloCaja = new Label();
            pnlTarjetaMorosos = new Panel();
            lblResumenDetalleMorosos = new Label();
            lblResumenValorMorosos = new Label();
            lblResumenTituloMorosos = new Label();
            pnlTarjetaSocios = new Panel();
            lblResumenDetalleSocios = new Label();
            lblResumenValorSocios = new Label();
            lblResumenTituloSocios = new Label();
            pnlTarjetaUsuarios = new Panel();
            lblResumenDetalleUsuarios = new Label();
            lblResumenValorUsuarios = new Label();
            lblResumenTituloUsuarios = new Label();
            tlpRaiz.SuspendLayout();
            flpTarjetas.SuspendLayout();
            pnlTarjetaCaja.SuspendLayout();
            pnlTarjetaMorosos.SuspendLayout();
            pnlTarjetaSocios.SuspendLayout();
            pnlTarjetaUsuarios.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.Controls.Add(lblSaludo, 0, 0);
            tlpRaiz.Controls.Add(lblDatoFecha, 0, 1);
            tlpRaiz.Controls.Add(lblAlerta, 0, 2);
            tlpRaiz.Controls.Add(flpTarjetas, 0, 3);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 30);
            tlpRaiz.RowCount = 5;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // lblSaludo
            //
            lblSaludo.AutoSize = true;
            lblSaludo.Margin = new Padding(3, 10, 3, 4);
            lblSaludo.Name = "lblSaludo";
            lblSaludo.Text = "Buen día";
            //
            // lblDatoFecha
            //
            lblDatoFecha.AutoSize = true;
            lblDatoFecha.Margin = new Padding(3, 0, 3, 20);
            lblDatoFecha.Name = "lblDatoFecha";
            lblDatoFecha.Text = "Hoy";
            //
            // lblAlerta
            //
            lblAlerta.AutoSize = true;
            lblAlerta.Margin = new Padding(3, 0, 3, 20);
            lblAlerta.Name = "lblAlerta";
            lblAlerta.Text = "Alerta";
            lblAlerta.Visible = false;
            //
            // flpTarjetas
            //
            flpTarjetas.AutoSize = true;
            flpTarjetas.Controls.Add(pnlTarjetaCaja);
            flpTarjetas.Controls.Add(pnlTarjetaMorosos);
            flpTarjetas.Controls.Add(pnlTarjetaSocios);
            flpTarjetas.Controls.Add(pnlTarjetaUsuarios);
            flpTarjetas.Dock = DockStyle.Fill;
            flpTarjetas.Name = "flpTarjetas";
            flpTarjetas.TabIndex = 0;
            //
            // pnlTarjetaCaja
            //
            pnlTarjetaCaja.Controls.Add(lblResumenDetalleCaja);
            pnlTarjetaCaja.Controls.Add(lblResumenValorCaja);
            pnlTarjetaCaja.Controls.Add(lblResumenTituloCaja);
            pnlTarjetaCaja.Margin = new Padding(0, 0, 20, 20);
            pnlTarjetaCaja.Name = "pnlTarjetaCaja";
            pnlTarjetaCaja.Padding = new Padding(18, 14, 18, 14);
            pnlTarjetaCaja.Size = new Size(280, 150);
            pnlTarjetaCaja.TabIndex = 0;
            //
            // lblResumenTituloCaja
            //
            lblResumenTituloCaja.Dock = DockStyle.Top;
            lblResumenTituloCaja.Height = 28;
            lblResumenTituloCaja.Name = "lblResumenTituloCaja";
            lblResumenTituloCaja.Text = "Caja";
            //
            // lblResumenValorCaja
            //
            lblResumenValorCaja.Dock = DockStyle.Top;
            lblResumenValorCaja.Height = 46;
            lblResumenValorCaja.Name = "lblResumenValorCaja";
            lblResumenValorCaja.Text = "-";
            //
            // lblResumenDetalleCaja
            //
            lblResumenDetalleCaja.Dock = DockStyle.Fill;
            lblResumenDetalleCaja.Name = "lblResumenDetalleCaja";
            lblResumenDetalleCaja.Text = "-";
            //
            // pnlTarjetaMorosos
            //
            pnlTarjetaMorosos.Controls.Add(lblResumenDetalleMorosos);
            pnlTarjetaMorosos.Controls.Add(lblResumenValorMorosos);
            pnlTarjetaMorosos.Controls.Add(lblResumenTituloMorosos);
            pnlTarjetaMorosos.Margin = new Padding(0, 0, 20, 20);
            pnlTarjetaMorosos.Name = "pnlTarjetaMorosos";
            pnlTarjetaMorosos.Padding = new Padding(18, 14, 18, 14);
            pnlTarjetaMorosos.Size = new Size(280, 150);
            pnlTarjetaMorosos.TabIndex = 1;
            //
            // lblResumenTituloMorosos
            //
            lblResumenTituloMorosos.Dock = DockStyle.Top;
            lblResumenTituloMorosos.Height = 28;
            lblResumenTituloMorosos.Name = "lblResumenTituloMorosos";
            lblResumenTituloMorosos.Text = "Socios que deben cuotas";
            //
            // lblResumenValorMorosos
            //
            lblResumenValorMorosos.Dock = DockStyle.Top;
            lblResumenValorMorosos.Height = 46;
            lblResumenValorMorosos.Name = "lblResumenValorMorosos";
            lblResumenValorMorosos.Text = "-";
            //
            // lblResumenDetalleMorosos
            //
            lblResumenDetalleMorosos.Dock = DockStyle.Fill;
            lblResumenDetalleMorosos.Name = "lblResumenDetalleMorosos";
            lblResumenDetalleMorosos.Text = "-";
            //
            // pnlTarjetaSocios
            //
            pnlTarjetaSocios.Controls.Add(lblResumenDetalleSocios);
            pnlTarjetaSocios.Controls.Add(lblResumenValorSocios);
            pnlTarjetaSocios.Controls.Add(lblResumenTituloSocios);
            pnlTarjetaSocios.Margin = new Padding(0, 0, 20, 20);
            pnlTarjetaSocios.Name = "pnlTarjetaSocios";
            pnlTarjetaSocios.Padding = new Padding(18, 14, 18, 14);
            pnlTarjetaSocios.Size = new Size(280, 150);
            pnlTarjetaSocios.TabIndex = 2;
            //
            // lblResumenTituloSocios
            //
            lblResumenTituloSocios.Dock = DockStyle.Top;
            lblResumenTituloSocios.Height = 28;
            lblResumenTituloSocios.Name = "lblResumenTituloSocios";
            lblResumenTituloSocios.Text = "Socios activos";
            //
            // lblResumenValorSocios
            //
            lblResumenValorSocios.Dock = DockStyle.Top;
            lblResumenValorSocios.Height = 46;
            lblResumenValorSocios.Name = "lblResumenValorSocios";
            lblResumenValorSocios.Text = "-";
            //
            // lblResumenDetalleSocios
            //
            lblResumenDetalleSocios.Dock = DockStyle.Fill;
            lblResumenDetalleSocios.Name = "lblResumenDetalleSocios";
            lblResumenDetalleSocios.Text = "-";
            //
            // pnlTarjetaUsuarios
            //
            pnlTarjetaUsuarios.Controls.Add(lblResumenDetalleUsuarios);
            pnlTarjetaUsuarios.Controls.Add(lblResumenValorUsuarios);
            pnlTarjetaUsuarios.Controls.Add(lblResumenTituloUsuarios);
            pnlTarjetaUsuarios.Margin = new Padding(0, 0, 20, 20);
            pnlTarjetaUsuarios.Name = "pnlTarjetaUsuarios";
            pnlTarjetaUsuarios.Padding = new Padding(18, 14, 18, 14);
            pnlTarjetaUsuarios.Size = new Size(280, 150);
            pnlTarjetaUsuarios.TabIndex = 3;
            //
            // lblResumenTituloUsuarios
            //
            lblResumenTituloUsuarios.Dock = DockStyle.Top;
            lblResumenTituloUsuarios.Height = 28;
            lblResumenTituloUsuarios.Name = "lblResumenTituloUsuarios";
            lblResumenTituloUsuarios.Text = "Usuarios activos";
            //
            // lblResumenValorUsuarios
            //
            lblResumenValorUsuarios.Dock = DockStyle.Top;
            lblResumenValorUsuarios.Height = 46;
            lblResumenValorUsuarios.Name = "lblResumenValorUsuarios";
            lblResumenValorUsuarios.Text = "-";
            //
            // lblResumenDetalleUsuarios
            //
            lblResumenDetalleUsuarios.Dock = DockStyle.Fill;
            lblResumenDetalleUsuarios.Name = "lblResumenDetalleUsuarios";
            lblResumenDetalleUsuarios.Text = "-";
            //
            // FrmInicio
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpRaiz);
            Name = "FrmInicio";
            Text = "Inicio";
            WindowState = FormWindowState.Maximized;
            Load += FrmInicio_Load;
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpTarjetas.ResumeLayout(false);
            pnlTarjetaCaja.ResumeLayout(false);
            pnlTarjetaMorosos.ResumeLayout(false);
            pnlTarjetaSocios.ResumeLayout(false);
            pnlTarjetaUsuarios.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblSaludo;
        private Label lblDatoFecha;
        private Label lblAlerta;
        private FlowLayoutPanel flpTarjetas;
        private Panel pnlTarjetaCaja;
        private Label lblResumenTituloCaja;
        private Label lblResumenValorCaja;
        private Label lblResumenDetalleCaja;
        private Panel pnlTarjetaMorosos;
        private Label lblResumenTituloMorosos;
        private Label lblResumenValorMorosos;
        private Label lblResumenDetalleMorosos;
        private Panel pnlTarjetaSocios;
        private Label lblResumenTituloSocios;
        private Label lblResumenValorSocios;
        private Label lblResumenDetalleSocios;
        private Panel pnlTarjetaUsuarios;
        private Label lblResumenTituloUsuarios;
        private Label lblResumenValorUsuarios;
        private Label lblResumenDetalleUsuarios;
    }
}
