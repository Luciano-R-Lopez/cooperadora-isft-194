namespace CooperadoraISFT94.Forms
{
    partial class FrmCobrarCuotas
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
            lblTituloCobro = new Label();
            lblCuotas = new Label();
            lstCuotas = new CheckedListBox();
            lblTotalCobro = new Label();
            lblMedio = new Label();
            cmbMedio = new ComboBox();
            chkPagaResponsable = new CheckBox();
            flpBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnCobrar = new Button();
            tlpRaiz.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.Controls.Add(lblTituloCobro, 0, 0);
            tlpRaiz.Controls.Add(lblCuotas, 0, 1);
            tlpRaiz.Controls.Add(lstCuotas, 0, 2);
            tlpRaiz.Controls.Add(lblTotalCobro, 0, 3);
            tlpRaiz.Controls.Add(lblMedio, 0, 4);
            tlpRaiz.Controls.Add(cmbMedio, 0, 5);
            tlpRaiz.Controls.Add(chkPagaResponsable, 0, 6);
            tlpRaiz.Controls.Add(flpBotones, 0, 7);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 20, 24, 16);
            tlpRaiz.RowCount = 8;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblTituloCobro
            //
            lblTituloCobro.AutoSize = true;
            lblTituloCobro.MaximumSize = new Size(560, 0);
            lblTituloCobro.Margin = new Padding(3, 0, 3, 8);
            lblTituloCobro.Name = "lblTituloCobro";
            lblTituloCobro.Text = "Socio";
            //
            // lblCuotas
            //
            lblCuotas.AutoSize = true;
            lblCuotas.Margin = new Padding(3, 8, 3, 3);
            lblCuotas.Name = "lblCuotas";
            lblCuotas.Text = "Cuotas a cobrar (no hay recargo por pagar tarde): *";
            //
            // lstCuotas
            //
            lstCuotas.CheckOnClick = true;
            lstCuotas.Dock = DockStyle.Fill;
            lstCuotas.Name = "lstCuotas";
            lstCuotas.TabIndex = 0;
            lstCuotas.ItemCheck += lstCuotas_ItemCheck;
            //
            // lblTotalCobro
            //
            lblTotalCobro.AutoSize = true;
            lblTotalCobro.Margin = new Padding(3, 8, 3, 3);
            lblTotalCobro.Name = "lblTotalCobro";
            lblTotalCobro.Text = "Total a cobrar:";
            //
            // lblMedio
            //
            lblMedio.AutoSize = true;
            lblMedio.Margin = new Padding(3, 12, 3, 3);
            lblMedio.Name = "lblMedio";
            lblMedio.Text = "Medio de pago: *";
            //
            // cmbMedio
            //
            cmbMedio.Dock = DockStyle.Fill;
            cmbMedio.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedio.Name = "cmbMedio";
            cmbMedio.TabIndex = 1;
            //
            // chkPagaResponsable
            //
            chkPagaResponsable.AutoSize = true;
            chkPagaResponsable.Margin = new Padding(3, 12, 3, 3);
            chkPagaResponsable.Name = "chkPagaResponsable";
            chkPagaResponsable.TabIndex = 2;
            chkPagaResponsable.Text = "Paga su responsable";
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnCobrar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Margin = new Padding(3, 16, 3, 3);
            flpBotones.Name = "flpBotones";
            flpBotones.TabIndex = 3;
            flpBotones.WrapContents = false;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            //
            // btnCobrar
            //
            btnCobrar.Margin = new Padding(8, 0, 0, 0);
            btnCobrar.Name = "btnCobrar";
            btnCobrar.Size = new Size(140, 40);
            btnCobrar.TabIndex = 0;
            btnCobrar.Text = "Cobrar";
            btnCobrar.Click += btnCobrar_Click;
            //
            // FrmCobrarCuotas
            //
            AcceptButton = btnCobrar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(620, 520);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCobrarCuotas";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cobrar cuotas";
            flpBotones.ResumeLayout(false);
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblTituloCobro;
        private Label lblCuotas;
        private CheckedListBox lstCuotas;
        private Label lblTotalCobro;
        private Label lblMedio;
        private ComboBox cmbMedio;
        private CheckBox chkPagaResponsable;
        private FlowLayoutPanel flpBotones;
        private Button btnCancelar;
        private Button btnCobrar;
    }
}
