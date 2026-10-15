namespace CooperadoraISFT94.Views.Common
{
    /// <summary>
    /// Paleta de colores sobria y su aplicación a los controles. Las ventanas llaman a <see cref="Aplicar"/>
    /// una vez; los controles que se agregan después (por ejemplo, al navegar entre pantallas) también se estilan.
    /// </summary>
    public static class Tema
    {
        // Paleta: grises azulados neutros, un azul apagado para las acciones y verde/rojo suaves para estados.
        public static readonly Color Fondo = ColorTranslator.FromHtml("#F3F4F6");
        public static readonly Color Superficie = Color.White;
        public static readonly Color Borde = ColorTranslator.FromHtml("#D4D9E0");
        public static readonly Color Texto = ColorTranslator.FromHtml("#1F2933");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#6B7480");
        public static readonly Color Encabezado = ColorTranslator.FromHtml("#2D3A4A");
        public static readonly Color EncabezadoTexto = ColorTranslator.FromHtml("#F3F4F6");
        public static readonly Color Primario = ColorTranslator.FromHtml("#3D5A80");
        public static readonly Color PrimarioHover = ColorTranslator.FromHtml("#324A6A");
        public static readonly Color Seleccion = ColorTranslator.FromHtml("#DCE3EC");
        public static readonly Color EncabezadoGrilla = ColorTranslator.FromHtml("#E7EAEE");
        public static readonly Color Deshabilitado = ColorTranslator.FromHtml("#C9CED6");
        public static readonly Color Exito = ColorTranslator.FromHtml("#3F7A5A");
        public static readonly Color ExitoSuave = ColorTranslator.FromHtml("#E7F0EA");
        public static readonly Color Peligro = ColorTranslator.FromHtml("#A6463F");
        public static readonly Color PeligroSuave = ColorTranslator.FromHtml("#F5E6E4");

        /// <summary>Marca (en Tag) de los controles que definen sus propios colores y el tema no debe pisar.</summary>
        private static readonly object ColoresPropios = new();

        /// <summary>Marca (en Tag) de los botones secundarios: Volver, Cancelar, Cerrar sesión...</summary>
        private static readonly object BotonSecundario = new();

        /// <summary>El tema no cambia los colores de este control (sí los de sus hijos).</summary>
        public static T ConColoresPropios<T>(T control) where T : Control
        {
            control.Tag = ColoresPropios;
            return control;
        }

        /// <summary>Botón de acción secundaria: fondo claro con borde, en lugar del azul de las acciones principales.</summary>
        public static Button Secundario(Button boton)
        {
            boton.Tag = BotonSecundario;
            return boton;
        }

        /// <summary>Aplica el tema al control y a todos sus hijos, actuales y futuros.</summary>
        public static void Aplicar(Control control)
        {
            if (control.Tag != ColoresPropios)
                Estilar(control);

            foreach (Control hijo in control.Controls)
                Aplicar(hijo);

            control.ControlAdded -= AlAgregarControl;
            control.ControlAdded += AlAgregarControl;
        }

        private static void AlAgregarControl(object? sender, ControlEventArgs e)
        {
            if (e.Control is not null)
                Aplicar(e.Control);
        }

        private static void Estilar(Control control)
        {
            switch (control)
            {
                case Form form:
                    form.BackColor = Fondo;
                    form.ForeColor = Texto;
                    break;
                case Button boton:
                    EstilarBoton(boton);
                    break;
                case DataGridView grilla:
                    EstilarGrilla(grilla);
                    break;
                case TextBox texto:
                    texto.BackColor = Superficie;
                    texto.ForeColor = Texto;
                    texto.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case NumericUpDown numero:
                    numero.BackColor = Superficie;
                    numero.ForeColor = Texto;
                    numero.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case ComboBox combo:
                    combo.BackColor = Superficie;
                    combo.ForeColor = Texto;
                    combo.FlatStyle = FlatStyle.Flat;
                    break;
                case GroupBox grupo:
                    grupo.BackColor = Fondo;
                    grupo.ForeColor = TextoSecundario;
                    break;
                case CheckBox check:
                    check.ForeColor = Texto;
                    break;
                case Label etiqueta:
                    etiqueta.ForeColor = Texto;
                    break;
                case TabPage pagina:
                    pagina.BackColor = Fondo;
                    break;
                case Panel or UserControl or TableLayoutPanel:
                    control.BackColor = Fondo;
                    break;
            }
        }

        private static void EstilarBoton(Button boton)
        {
            var secundario = boton.Tag == BotonSecundario;

            boton.FlatStyle = FlatStyle.Flat;
            boton.Cursor = Cursors.Hand;
            boton.FlatAppearance.BorderSize = secundario ? 1 : 0;
            boton.FlatAppearance.BorderColor = Borde;
            boton.FlatAppearance.MouseOverBackColor = secundario ? Seleccion : PrimarioHover;
            boton.FlatAppearance.MouseDownBackColor = secundario ? Borde : Encabezado;

            void PintarSegunEstado()
            {
                if (secundario)
                {
                    boton.BackColor = Superficie;
                    boton.ForeColor = boton.Enabled ? Texto : TextoSecundario;
                }
                else
                {
                    boton.BackColor = boton.Enabled ? Primario : Deshabilitado;
                    boton.ForeColor = Color.White;
                }
            }

            PintarSegunEstado();
            boton.EnabledChanged += (_, _) => PintarSegunEstado();
        }

        private static void EstilarGrilla(DataGridView grilla)
        {
            grilla.BackgroundColor = Superficie;
            grilla.BorderStyle = BorderStyle.None;
            grilla.GridColor = Borde;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.EnableHeadersVisualStyles = false;

            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = 34;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = EncabezadoGrilla;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = EncabezadoGrilla;
            grilla.ColumnHeadersDefaultCellStyle.Font = new Font(grilla.Font, FontStyle.Bold);
            grilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);

            grilla.RowTemplate.Height = 30;
            grilla.DefaultCellStyle.BackColor = Superficie;
            grilla.DefaultCellStyle.ForeColor = Texto;
            grilla.DefaultCellStyle.SelectionBackColor = Seleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Texto;
            grilla.DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
        }
    }
}
