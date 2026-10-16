using System.Runtime.InteropServices;

namespace CooperadoraISFT94.Views.Common
{
    /// <summary>
    /// Paleta de colores oscura y sobria, y su aplicación a los controles. Las ventanas llaman a <see cref="Aplicar"/>
    /// una vez; los controles que se agregan después (por ejemplo, al navegar entre pantallas) también se estilan.
    /// </summary>
    public static class Tema
    {
        // Paleta oscura: grises azulados, un azul apagado para las acciones y verde/rojo apagados para estados.
        public static readonly Color Fondo = ColorTranslator.FromHtml("#1E2228");
        public static readonly Color Superficie = ColorTranslator.FromHtml("#272C34");
        public static readonly Color Borde = ColorTranslator.FromHtml("#3B424D");
        public static readonly Color Texto = ColorTranslator.FromHtml("#E3E6EA");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#9AA3AE");
        public static readonly Color Encabezado = ColorTranslator.FromHtml("#15181D");
        public static readonly Color EncabezadoTexto = ColorTranslator.FromHtml("#E3E6EA");
        public static readonly Color Primario = ColorTranslator.FromHtml("#45689A");
        public static readonly Color PrimarioHover = ColorTranslator.FromHtml("#5479AD");
        public static readonly Color Seleccion = ColorTranslator.FromHtml("#33435A");
        public static readonly Color EncabezadoGrilla = ColorTranslator.FromHtml("#2F353F");
        public static readonly Color Deshabilitado = ColorTranslator.FromHtml("#3B424D");
        public static readonly Color Exito = ColorTranslator.FromHtml("#3D7356");
        public static readonly Color ExitoSuave = ColorTranslator.FromHtml("#24352C");
        public static readonly Color Peligro = ColorTranslator.FromHtml("#9A4540");
        public static readonly Color PeligroSuave = ColorTranslator.FromHtml("#3A2729");
        /// <summary>Rojo más claro, legible como texto sobre el fondo oscuro.</summary>
        public static readonly Color PeligroTexto = ColorTranslator.FromHtml("#E58C83");

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

            // Las partes internas de un campo numérico (su cuadro de texto y sus flechas) las estila el propio campo.
            if (control is NumericUpDown)
                return;

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
                    UsarBarraDeTituloOscura(form);
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
                    EstilarCampoNumerico(numero);
                    break;
                case ComboBox combo:
                    EstilarDesplegable(combo);
                    break;
                case GroupBox grupo:
                    grupo.BackColor = Fondo;
                    grupo.ForeColor = TextoSecundario;
                    grupo.Paint -= PintarRecuadro;
                    grupo.Paint += PintarRecuadro;
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

        /// <summary>
        /// Las flechas de los campos numéricos son nativas y siempre claras: se redibujan encima
        /// (después de que Windows las pinta) con los colores del tema.
        /// </summary>
        private static void EstilarCampoNumerico(NumericUpDown numero)
        {
            numero.BackColor = Superficie;
            numero.ForeColor = Texto;
            numero.BorderStyle = BorderStyle.FixedSingle;

            foreach (Control parte in numero.Controls)
            {
                parte.BackColor = Superficie;
                parte.ForeColor = Texto;

                if (parte is not TextBox)
                {
                    parte.Paint -= PintarFlechas;
                    parte.Paint += PintarFlechas;
                    // Al pasar el mouse o presionar, Windows las vuelve a pintar; se repinta todo para cubrirlo.
                    parte.MouseMove += (_, _) => parte.Invalidate();
                    parte.MouseUp += (_, _) => parte.Invalidate();
                    parte.MouseLeave += (_, _) => parte.Invalidate();
                }
            }

            numero.EnabledChanged += (_, _) =>
            {
                foreach (Control parte in numero.Controls)
                    parte.Invalidate();
            };
        }

        private static void PintarFlechas(object? sender, PaintEventArgs e)
        {
            if (sender is not Control flechas)
                return;

            var g = e.Graphics;
            var area = flechas.ClientRectangle;
            var mitad = area.Height / 2;
            var puntero = flechas.PointToClient(Cursor.Position);
            var sobre = area.Contains(puntero);

            g.Clear(Superficie);

            // Resalta la flecha que está bajo el mouse.
            if (sobre)
            {
                using var resaltado = new SolidBrush(Seleccion);
                g.FillRectangle(resaltado, 0, puntero.Y < mitad ? 0 : mitad, area.Width, puntero.Y < mitad ? mitad : area.Height - mitad);
            }

            using (var borde = new Pen(Borde))
            {
                g.DrawLine(borde, 0, 0, 0, area.Height);
                g.DrawLine(borde, 0, mitad, area.Width, mitad);
            }

            var habilitado = flechas.Parent?.Enabled ?? true;
            using var pincel = new SolidBrush(habilitado ? Texto : TextoSecundario);
            var centroX = area.Width / 2f;
            const float anchoMedio = 3.5f, alto = 3f;

            var arriba = mitad / 2f;
            g.FillPolygon(pincel, new[]
            {
                new PointF(centroX - anchoMedio, arriba + alto / 2), new PointF(centroX + anchoMedio, arriba + alto / 2),
                new PointF(centroX, arriba - alto / 2)
            });

            var abajo = mitad + (area.Height - mitad) / 2f;
            g.FillPolygon(pincel, new[]
            {
                new PointF(centroX - anchoMedio, abajo - alto / 2), new PointF(centroX + anchoMedio, abajo - alto / 2),
                new PointF(centroX, abajo + alto / 2)
            });
        }

        /// <summary>
        /// Windows pinta en blanco la parte visible de los desplegables (DropDownList) sin importar su BackColor,
        /// así que sus elementos se dibujan a mano con los colores del tema.
        /// </summary>
        private static void EstilarDesplegable(ComboBox combo)
        {
            combo.BackColor = Superficie;
            combo.ForeColor = Texto;
            combo.FlatStyle = FlatStyle.Flat;
            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.DrawItem -= DibujarElementoDesplegable;
            combo.DrawItem += DibujarElementoDesplegable;
        }

        private static void DibujarElementoDesplegable(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox combo)
                return;

            // Resaltado solo dentro de la lista abierta; la parte cerrada del desplegable queda lisa.
            var resaltado = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
            using (var fondo = new SolidBrush(resaltado ? Seleccion : Superficie))
                e.Graphics.FillRectangle(fondo, e.Bounds);

            if (e.Index >= 0)
            {
                TextRenderer.DrawText(e.Graphics, combo.GetItemText(combo.Items[e.Index]), combo.Font, e.Bounds,
                    combo.Enabled ? Texto : TextoSecundario,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        /// <summary>El borde nativo de los GroupBox es blanco: se redibuja con el color de borde del tema.</summary>
        private static void PintarRecuadro(object? sender, PaintEventArgs e)
        {
            if (sender is not GroupBox grupo)
                return;

            var texto = TextRenderer.MeasureText(e.Graphics, grupo.Text, grupo.Font);
            var arriba = texto.Height / 2;

            e.Graphics.Clear(grupo.BackColor);
            using (var borde = new Pen(Borde))
                e.Graphics.DrawRectangle(borde, 0, arriba, grupo.Width - 1, grupo.Height - arriba - 1);

            if (grupo.Text.Length > 0)
            {
                var areaTexto = new Rectangle(8, 0, texto.Width, texto.Height);
                using (var fondo = new SolidBrush(grupo.BackColor))
                    e.Graphics.FillRectangle(fondo, areaTexto);
                TextRenderer.DrawText(e.Graphics, grupo.Text, grupo.Font, areaTexto, grupo.ForeColor);
            }
        }

        /// <summary>Pide a Windows (10 20H1 en adelante) la barra de título oscura; en versiones viejas no hace nada.</summary>
        private static void UsarBarraDeTituloOscura(Form form)
        {
            void Activar()
            {
                var activado = 1;
                _ = DwmSetWindowAttribute(form.Handle, DwmwaUseImmersiveDarkMode, ref activado, sizeof(int));
            }

            if (form.IsHandleCreated)
                Activar();
            else
                form.HandleCreated += (_, _) => Activar();
        }

        private const int DwmwaUseImmersiveDarkMode = 20;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamanio);

        private static void EstilarBoton(Button boton)
        {
            var secundario = boton.Tag == BotonSecundario;

            boton.FlatStyle = FlatStyle.Flat;
            boton.Cursor = Cursors.Hand;
            boton.FlatAppearance.BorderSize = secundario ? 1 : 0;
            boton.FlatAppearance.BorderColor = Borde;
            boton.FlatAppearance.MouseOverBackColor = secundario ? Seleccion : PrimarioHover;
            boton.FlatAppearance.MouseDownBackColor = secundario ? Borde : Primario;

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
