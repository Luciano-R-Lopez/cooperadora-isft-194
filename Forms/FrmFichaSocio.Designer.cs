namespace CooperadoraISFT94.Forms
{
    partial class FrmFichaSocio
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
            lblTituloSocio = new Label();
            btnVolver = new Button();
            lblDatoSocio = new Label();
            lblDatoVinculo = new Label();
            lblDgvCuotas = new Label();
            dgvCuotas = new DataGridView();
            flpAcciones = new FlowLayoutPanel();
            btnCobrar = new Button();
            btnGenerarCuota = new Button();
            lblAviso = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvCuotas).BeginInit();
            tlpRaiz.SuspendLayout();
            flpAcciones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 2;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.Controls.Add(lblTituloSocio, 0, 0);
            tlpRaiz.Controls.Add(btnVolver, 1, 0);
            tlpRaiz.Controls.Add(lblDatoSocio, 0, 1);
            tlpRaiz.SetColumnSpan(lblDatoSocio, 2);
            tlpRaiz.Controls.Add(lblDatoVinculo, 0, 2);
            tlpRaiz.SetColumnSpan(lblDatoVinculo, 2);
            tlpRaiz.Controls.Add(lblDgvCuotas, 0, 3);
            tlpRaiz.SetColumnSpan(lblDgvCuotas, 2);
            tlpRaiz.Controls.Add(dgvCuotas, 0, 4);
            tlpRaiz.SetColumnSpan(dgvCuotas, 2);
            tlpRaiz.Controls.Add(flpAcciones, 0, 5);
            tlpRaiz.SetColumnSpan(flpAcciones, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(40, 10, 40, 30);
            tlpRaiz.RowCount = 6;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblTituloSocio
            //
            lblTituloSocio.Anchor = AnchorStyles.Left;
            lblTituloSocio.AutoSize = true;
            lblTituloSocio.Name = "lblTituloSocio";
            lblTituloSocio.Text = "Socio";
            //
            // btnVolver
            //
            btnVolver.Anchor = AnchorStyles.Right;
            btnVolver.Margin = new Padding(3, 3, 3, 10);
            btnVolver.Name = "btnVolver";
            btnVolver.Size = new Size(110, 38);
            btnVolver.TabIndex = 10;
            btnVolver.Text = "Volver";
            btnVolver.Click += btnVolver_Click;
            //
            // lblDatoSocio
            //
            lblDatoSocio.AutoSize = true;
            lblDatoSocio.Margin = new Padding(3, 4, 3, 3);
            lblDatoSocio.Name = "lblDatoSocio";
            lblDatoSocio.Text = "DNI | Categoría | Teléfono | Email";
            //
            // lblDatoVinculo
            //
            lblDatoVinculo.AutoSize = true;
            lblDatoVinculo.Margin = new Padding(3, 4, 3, 3);
            lblDatoVinculo.Name = "lblDatoVinculo";
            lblDatoVinculo.Text = "Responsable";
            //
            // lblDgvCuotas
            //
            lblDgvCuotas.AutoSize = true;
            lblDgvCuotas.Margin = new Padding(3, 18, 3, 6);
            lblDgvCuotas.Name = "lblDgvCuotas";
            lblDgvCuotas.Text = "Cuotas";
            //
            // dgvCuotas
            //
            dgvCuotas.AllowUserToAddRows = false;
            dgvCuotas.AllowUserToDeleteRows = false;
            dgvCuotas.AllowUserToResizeRows = false;
            dgvCuotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCuotas.Dock = DockStyle.Fill;
            dgvCuotas.MultiSelect = false;
            dgvCuotas.Name = "dgvCuotas";
            dgvCuotas.ReadOnly = true;
            dgvCuotas.RowHeadersVisible = false;
            dgvCuotas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCuotas.TabIndex = 0;
            dgvCuotas.DataBindingComplete += dgvCuotas_DataBindingComplete;
            //
            // flpAcciones
            //
            flpAcciones.AutoSize = true;
            flpAcciones.Controls.Add(btnCobrar);
            flpAcciones.Controls.Add(btnGenerarCuota);
            flpAcciones.Controls.Add(lblAviso);
            flpAcciones.Dock = DockStyle.Fill;
            flpAcciones.Margin = new Padding(0, 10, 0, 0);
            flpAcciones.Name = "flpAcciones";
            flpAcciones.TabIndex = 1;
            //
            // btnCobrar
            //
            btnCobrar.Margin = new Padding(3, 0, 8, 0);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(200, 40);
            btnCobrar.TabIndex = 0;
            btnCobrar.Text = "Cobrar cuotas";
            btnCobrar.Click += btnCobrar_Click;
            //
            // btnGenerarCuota
            //
            btnGenerarCuota.Margin = new Padding(3, 0, 8, 0);
            btnGenerarCuota.Name = "btnGenerarCuota";
            btnGenerarCuota.Size = new Size(160, 40);
            btnGenerarCuota.TabIndex = 1;
            btnGenerarCuota.Text = "Generar cuota";
            btnGenerarCuota.Click += btnGenerarCuota_Click;
            //
            // lblAviso
            //
            lblAviso.AutoSize = true;
            lblAviso.Margin = new Padding(12, 10, 3, 0);
            lblAviso.Name = "lblAviso";
            //
            // FrmFichaSocio
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(tlpRaiz);
            MinimumSize = new Size(900, 600);
            Name = "FrmFichaSocio";
            Text = "Ficha del socio";
            Load += FrmFichaSocio_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCuotas).EndInit();
            flpAcciones.ResumeLayout(false);
            flpAcciones.PerformLayout();
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblTituloSocio;
        private Button btnVolver;
        private Label lblDatoSocio;
        private Label lblDatoVinculo;
        private Label lblDgvCuotas;
        private DataGridView dgvCuotas;
        private FlowLayoutPanel flpAcciones;
        private Button btnCobrar;
        private Button btnGenerarCuota;
        private Label lblAviso;
    }
}
