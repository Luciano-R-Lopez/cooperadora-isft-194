namespace CooperadoraISFT94.Forms
{
    partial class FrmTexto
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
            lblDatoPregunta = new Label();
            txtTexto = new TextBox();
            flpBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnAceptar = new Button();
            tlpRaiz.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.Controls.Add(lblDatoPregunta, 0, 0);
            tlpRaiz.Controls.Add(txtTexto, 0, 1);
            tlpRaiz.Controls.Add(flpBotones, 0, 2);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 20, 24, 16);
            tlpRaiz.RowCount = 3;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // lblDatoPregunta
            //
            lblDatoPregunta.AutoSize = true;
            lblDatoPregunta.Margin = new Padding(3, 0, 3, 10);
            lblDatoPregunta.MaximumSize = new Size(440, 0);
            lblDatoPregunta.Name = "lblDatoPregunta";
            lblDatoPregunta.Text = "Pregunta";
            //
            // txtTexto
            //
            txtTexto.Dock = DockStyle.Fill;
            txtTexto.MaxLength = 200;
            txtTexto.Name = "txtTexto";
            txtTexto.TabIndex = 0;
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnAceptar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Margin = new Padding(3, 16, 3, 3);
            flpBotones.Name = "flpBotones";
            flpBotones.TabIndex = 1;
            flpBotones.WrapContents = false;
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 40);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            //
            // btnAceptar
            //
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Margin = new Padding(8, 0, 0, 0);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 40);
            btnAceptar.TabIndex = 0;
            btnAceptar.Text = "Aceptar";
            //
            // FrmTexto
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(500, 260);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmTexto";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Texto";
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblDatoPregunta;
        private TextBox txtTexto;
        private FlowLayoutPanel flpBotones;
        private Button btnCancelar;
        private Button btnAceptar;
    }
}
