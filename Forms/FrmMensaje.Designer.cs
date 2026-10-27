namespace CooperadoraISFT94.Forms
{
    partial class FrmMensaje
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
            lblDatoTexto = new Label();
            flpBotones = new FlowLayoutPanel();
            btnAceptar = new Button();
            btnCancelar = new Button();
            tlpRaiz.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.AutoSize = true;
            tlpRaiz.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            tlpRaiz.Controls.Add(lblDatoTexto, 0, 0);
            tlpRaiz.Controls.Add(flpBotones, 0, 1);
            tlpRaiz.Location = new Point(0, 0);
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 24, 24, 16);
            tlpRaiz.RowCount = 2;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.TabIndex = 0;
            //
            // lblDatoTexto
            //
            lblDatoTexto.AutoSize = true;
            lblDatoTexto.MaximumSize = new Size(460, 0);
            lblDatoTexto.MinimumSize = new Size(320, 0);
            lblDatoTexto.Margin = new Padding(3, 0, 3, 20);
            lblDatoTexto.Name = "lblDatoTexto";
            lblDatoTexto.TabIndex = 0;
            lblDatoTexto.Text = "Texto del mensaje";
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnAceptar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Name = "flpBotones";
            flpBotones.TabIndex = 1;
            flpBotones.WrapContents = false;
            //
            // btnAceptar
            //
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Margin = new Padding(8, 0, 0, 0);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(110, 38);
            btnAceptar.TabIndex = 0;
            btnAceptar.Text = "Aceptar";
            //
            // btnCancelar
            //
            btnCancelar.DialogResult = DialogResult.Cancel;
            btnCancelar.Margin = new Padding(8, 0, 0, 0);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(110, 38);
            btnCancelar.TabIndex = 1;
            btnCancelar.Text = "Cancelar";
            //
            // FrmMensaje
            //
            AcceptButton = btnAceptar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            CancelButton = btnCancelar;
            ClientSize = new Size(400, 150);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmMensaje";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Mensaje";
            flpBotones.ResumeLayout(false);
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblDatoTexto;
        private FlowLayoutPanel flpBotones;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}
