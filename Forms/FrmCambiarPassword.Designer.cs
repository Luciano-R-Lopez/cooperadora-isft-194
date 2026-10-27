namespace CooperadoraISFT94.Forms
{
    partial class FrmCambiarPassword
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
            lblActual = new Label();
            txtActual = new TextBox();
            lblNueva = new Label();
            txtNueva = new TextBox();
            lblConfirmacion = new Label();
            txtConfirmacion = new TextBox();
            flpBotones = new FlowLayoutPanel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            tlpRaiz.SuspendLayout();
            flpBotones.SuspendLayout();
            SuspendLayout();
            //
            // tlpRaiz
            //
            tlpRaiz.ColumnCount = 1;
            tlpRaiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRaiz.Controls.Add(lblActual, 0, 0);
            tlpRaiz.Controls.Add(txtActual, 0, 1);
            tlpRaiz.Controls.Add(lblNueva, 0, 2);
            tlpRaiz.Controls.Add(txtNueva, 0, 3);
            tlpRaiz.Controls.Add(lblConfirmacion, 0, 4);
            tlpRaiz.Controls.Add(txtConfirmacion, 0, 5);
            tlpRaiz.Controls.Add(flpBotones, 0, 6);
            tlpRaiz.Dock = DockStyle.Fill;
            tlpRaiz.Name = "tlpRaiz";
            tlpRaiz.Padding = new Padding(24, 16, 24, 16);
            tlpRaiz.RowCount = 7;
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpRaiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRaiz.TabIndex = 0;
            //
            // lblActual
            //
            lblActual.AutoSize = true;
            lblActual.Margin = new Padding(3, 8, 3, 3);
            lblActual.Name = "lblActual";
            lblActual.Text = "Contraseña actual: *";
            //
            // txtActual
            //
            txtActual.Dock = DockStyle.Fill;
            txtActual.Name = "txtActual";
            txtActual.TabIndex = 0;
            txtActual.UseSystemPasswordChar = true;
            //
            // lblNueva
            //
            lblNueva.AutoSize = true;
            lblNueva.Margin = new Padding(3, 8, 3, 3);
            lblNueva.Name = "lblNueva";
            lblNueva.Text = "Nueva contraseña (6 a 64 caracteres): *";
            //
            // txtNueva
            //
            txtNueva.Dock = DockStyle.Fill;
            txtNueva.Name = "txtNueva";
            txtNueva.TabIndex = 1;
            txtNueva.UseSystemPasswordChar = true;
            //
            // lblConfirmacion
            //
            lblConfirmacion.AutoSize = true;
            lblConfirmacion.Margin = new Padding(3, 8, 3, 3);
            lblConfirmacion.Name = "lblConfirmacion";
            lblConfirmacion.Text = "Repita la nueva contraseña: *";
            //
            // txtConfirmacion
            //
            txtConfirmacion.Dock = DockStyle.Fill;
            txtConfirmacion.Name = "txtConfirmacion";
            txtConfirmacion.TabIndex = 2;
            txtConfirmacion.UseSystemPasswordChar = true;
            //
            // flpBotones
            //
            flpBotones.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            flpBotones.AutoSize = true;
            flpBotones.Controls.Add(btnCancelar);
            flpBotones.Controls.Add(btnGuardar);
            flpBotones.FlowDirection = FlowDirection.RightToLeft;
            flpBotones.Name = "flpBotones";
            flpBotones.TabIndex = 3;
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
            // btnGuardar
            //
            btnGuardar.Margin = new Padding(8, 0, 0, 0);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(120, 40);
            btnGuardar.TabIndex = 0;
            btnGuardar.Text = "Guardar";
            btnGuardar.Click += btnGuardar_Click;
            //
            // FrmCambiarPassword
            //
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancelar;
            ClientSize = new Size(440, 330);
            Controls.Add(tlpRaiz);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCambiarPassword";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cambiar mi contraseña";
            tlpRaiz.ResumeLayout(false);
            tlpRaiz.PerformLayout();
            flpBotones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRaiz;
        private Label lblActual;
        private TextBox txtActual;
        private Label lblNueva;
        private TextBox txtNueva;
        private Label lblConfirmacion;
        private TextBox txtConfirmacion;
        private FlowLayoutPanel flpBotones;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}
