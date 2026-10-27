using System.Runtime.InteropServices;

namespace CooperadoraISFT94.Forms.Utils
{
    // Tema oscuro y sobrio de toda la aplicación: colores, letras y estilo de cada control. Cada formulario llama a
    // Tema.Aplicar(this) al abrirse; los controles que se agregan después también se estilan.
    // Como en el proyecto Eventos, algunos estilos dependen del nombre del control (btnGuardar, lblModo, lblAviso...).
    public static class Tema
    {
        public static readonly Color Fondo = ColorTranslator.FromHtml("#1E2228");
        public static readonly Color Superficie = ColorTranslator.FromHtml("#272C34");
        public static readonly Color Borde = ColorTranslator.FromHtml("#3B424D");
        public static readonly Color Texto = ColorTranslator.FromHtml("#E3E6EA");
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#9AA3AE");
        public static readonly Color Menu = ColorTranslator.FromHtml("#15181D");
        public static readonly Color MenuHover = ColorTranslator.FromHtml("#232831");
        public static readonly Color Primario = ColorTranslator.FromHtml("#45689A");
        public static readonly Color PrimarioHover = ColorTranslator.FromHtml("#5479AD");
        public static readonly Color Acento = ColorTranslator.FromHtml("#8FB3E6");
        public static readonly Color Seleccion = ColorTranslator.FromHtml("#33435A");
        public static readonly Color EncabezadoGrilla = ColorTranslator.FromHtml("#2F353F");
        public static readonly Color Deshabilitado = ColorTranslator.FromHtml("#3B424D");
        public static readonly Color Exito = ColorTranslator.FromHtml("#3D7356");
        public static readonly Color ExitoSuave = ColorTranslator.FromHtml("#24352C");
        public static readonly Color ExitoTexto = ColorTranslator.FromHtml("#8FD1A8");
        public static readonly Color Peligro = ColorTranslator.FromHtml("#9A4540");
        public static readonly Color PeligroHover = ColorTranslator.FromHtml("#AD524C");
        public static readonly Color PeligroSuave = ColorTranslator.FromHtml("#3A2729");
        public static readonly Color PeligroTexto = ColorTranslator.FromHtml("#E58C83");

        private static readonly Font FuenteNormal = new Font("Segoe UI", 10F);
        private static readonly Font FuenteCampo = new Font("Segoe UI", 10.5F);
        private static readonly Font FuenteNegrita = new Font("Segoe UI", 10F, FontStyle.Bold);
        private static readonly Font FuenteTitulo = new Font("Segoe UI Semibold", 13F);
        private static readonly Font FuenteGrande = new Font("Segoe UI Semibold", 18F);
        private static readonly Font FuenteMenu = new Font("Segoe UI", 10.5F);

        // Botones de acción principal (azules) y de acciones que no se pueden deshacer (rojos). El resto es secundario.
        private static readonly string[] BotonesPrincipales =
        {
            "btnGuardar", "btnIngresar", "btnRegistrar", "btnCobrar", "btnAccionCaja", "btnAceptar", "btnGenerar"
        };
        private static readonly string[] BotonesPeligro = { "btnAnular" };

        // Marca (en Tag) de los controles que pintan sus propios colores.
        public const string ColoresPropios = "ColoresPropios";

        public static void Aplicar(Control control)
        {
            if (!ColoresPropios.Equals(control.Tag))
            {
                Estilar(control);
            }

            // Las partes internas de un campo numérico (su cuadro de texto y sus flechas) las estila el propio campo.
            if (control is NumericUpDown || control is CampoFecha)
            {
                return;
            }

            foreach (Control hijo in control.Controls)
            {
                Aplicar(hijo);
            }
            control.ControlAdded -= AlAgregarControl;
            control.ControlAdded += AlAgregarControl;
        }

        // Menú lateral de la ventana principal, su título y el encabezado con el nombre de la pantalla abierta.
        public static void EstilarMenu(Panel menu, Label titulo, Label usuario, Panel encabezado, Label pantalla)
        {
            menu.BackColor = Menu;
            titulo.ForeColor = Texto;
            titulo.Font = new Font("Segoe UI Semibold", 12F);
            usuario.ForeColor = TextoSecundario;
            usuario.Font = FuenteNormal;
            encabezado.BackColor = Fondo;
            pantalla.ForeColor = Texto;
            pantalla.Font = FuenteGrande;
            foreach (Control control in menu.Controls)
            {
                if (control is Button boton)
                {
                    boton.FlatStyle = FlatStyle.Flat;
                    boton.FlatAppearance.BorderSize = 0;
                    boton.FlatAppearance.MouseOverBackColor = MenuHover;
                    boton.FlatAppearance.MouseDownBackColor = MenuHover;
                    boton.BackColor = Menu;
                    boton.ForeColor = TextoSecundario;
                    boton.Font = FuenteMenu;
                    boton.Cursor = Cursors.Hand;
                    boton.Paint -= DibujarMarcaActivo;
                    boton.Paint += DibujarMarcaActivo;
                }
            }
            UsarTemaOscuroDeWindows(menu, "DarkMode_Explorer");
            UsarBarraDeTituloOscura(menu.FindForm());
        }

        public static void MarcarBotonMenu(Button boton, bool activo)
        {
            boton.Tag = activo ? "activo" : null;
            boton.ForeColor = activo ? Texto : TextoSecundario;
            boton.BackColor = activo ? MenuHover : Menu;
            boton.Invalidate();
        }

        // Franja que indica si hay una caja abierta (las pantallas de cobro y de movimientos la necesitan).
        public static void MostrarEstadoCaja(Label etiqueta, bool abierta)
        {
            etiqueta.Text = abierta ? "CAJA ABIERTA" : "CAJA CERRADA: el contador debe abrirla para registrar movimientos";
            etiqueta.BackColor = abierta ? ExitoSuave : PeligroSuave;
            etiqueta.ForeColor = abierta ? ExitoTexto : PeligroTexto;
        }

        public static void AtenuarFila(DataGridViewRow fila)
        {
            fila.DefaultCellStyle.ForeColor = TextoSecundario;
        }

        public static void ResaltarFila(DataGridViewRow fila)
        {
            fila.DefaultCellStyle.BackColor = ExitoSuave;
        }

        private static void AlAgregarControl(object? sender, ControlEventArgs e)
        {
            if (e.Control != null)
            {
                Aplicar(e.Control);
            }
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
                    texto.Font = FuenteCampo;
                    break;
                case NumericUpDown numero:
                    EstilarCampoNumerico(numero);
                    break;
                case CampoFecha fecha:
                    fecha.Font = FuenteCampo;
                    break;
                case ComboBox combo:
                    EstilarDesplegable(combo);
                    break;
                case CheckedListBox lista:
                    lista.BackColor = Superficie;
                    lista.ForeColor = Texto;
                    lista.BorderStyle = BorderStyle.FixedSingle;
                    lista.Font = FuenteCampo;
                    break;
                case CheckBox check:
                    check.ForeColor = Texto;
                    check.Font = FuenteNormal;
                    break;
                case Label etiqueta:
                    EstilarEtiqueta(etiqueta);
                    break;
                case Panel panel when EstaEnTarjeta(panel):
                    panel.BackColor = Superficie;
                    break;
                case Panel:
                    control.BackColor = Fondo;
                    if (control is ScrollableControl desplazable && desplazable.AutoScroll)
                    {
                        UsarTemaOscuroDeWindows(control, "DarkMode_Explorer");
                    }
                    break;
            }
        }

        // Las tarjetas (paneles pnlTarjeta...) y lo que tienen adentro van sobre el color de superficie.
        private static bool EstaEnTarjeta(Control control)
        {
            for (Control? actual = control; actual != null; actual = actual.Parent)
            {
                if (actual.Name.StartsWith("pnlTarjeta", StringComparison.Ordinal))
                {
                    return true;
                }
            }
            return false;
        }

        private static void EstilarEtiqueta(Label etiqueta)
        {
            string nombre = etiqueta.Name;
            etiqueta.BackColor = Color.Transparent;

            if (nombre == "lblModo" || nombre.StartsWith("lblDgv", StringComparison.Ordinal)
                || nombre.StartsWith("lblTitulo", StringComparison.Ordinal))
            {
                etiqueta.Font = FuenteTitulo;
                etiqueta.ForeColor = Acento;
            }
            else if (nombre == "lblAviso")
            {
                etiqueta.Font = FuenteNegrita;
                etiqueta.ForeColor = ExitoTexto;
            }
            else if (nombre == "lblAlerta")
            {
                etiqueta.Font = FuenteNegrita;
                etiqueta.BackColor = PeligroSuave;
                etiqueta.ForeColor = PeligroTexto;
                etiqueta.Padding = new Padding(14, 10, 14, 10);
            }
            else if (nombre == "lblEstadoCaja")
            {
                etiqueta.Font = FuenteNegrita;
                etiqueta.TextAlign = ContentAlignment.MiddleCenter;
            }
            else if (nombre == "lblSaludo" || nombre.StartsWith("lblResumenValor", StringComparison.Ordinal))
            {
                etiqueta.Font = FuenteGrande;
                etiqueta.ForeColor = Texto;
            }
            else if (nombre.StartsWith("lblResumenTitulo", StringComparison.Ordinal) || nombre.StartsWith("lblTotal", StringComparison.Ordinal))
            {
                etiqueta.Font = FuenteNegrita;
                etiqueta.ForeColor = Texto;
            }
            else if (nombre.StartsWith("lblDato", StringComparison.Ordinal) || nombre.StartsWith("lblResumenDetalle", StringComparison.Ordinal))
            {
                etiqueta.Font = FuenteNormal;
                etiqueta.ForeColor = Texto;
            }
            else
            {
                etiqueta.Font = FuenteNormal;
                etiqueta.ForeColor = TextoSecundario;
            }
        }

        private static void EstilarBoton(Button boton)
        {
            boton.UseVisualStyleBackColor = false;
            boton.FlatStyle = FlatStyle.Flat;
            boton.Cursor = Cursors.Hand;
            boton.Font = FuenteNegrita;
            boton.FlatAppearance.BorderSize = 1;

            // Como en Eventos: el botón se agranda si el texto no entra (salvo que lo acomode su contenedor).
            if (boton.Dock == DockStyle.None)
            {
                Size medida = TextRenderer.MeasureText(boton.Text, FuenteNegrita);
                boton.Height = Math.Max(boton.Height, Math.Max(38, medida.Height + 14));
                boton.Width = Math.Max(boton.Width, medida.Width + 32);
            }

            Color fondo;
            Color hover;
            Color texto = Color.White;
            if (BotonesPrincipales.Contains(boton.Name))
            {
                fondo = Primario;
                hover = PrimarioHover;
            }
            else if (BotonesPeligro.Contains(boton.Name))
            {
                fondo = Peligro;
                hover = PeligroHover;
            }
            else
            {
                fondo = Superficie;
                hover = Seleccion;
                texto = Texto;
            }
            boton.BackColor = fondo;
            boton.ForeColor = texto;
            boton.FlatAppearance.BorderColor = fondo == Superficie ? Borde : fondo;
            boton.FlatAppearance.MouseOverBackColor = hover;
            boton.FlatAppearance.MouseDownBackColor = hover;

            // Deshabilitado se ve apagado.
            boton.EnabledChanged -= ActualizarBotonHabilitado;
            boton.EnabledChanged += ActualizarBotonHabilitado;
            ActualizarBotonHabilitado(boton, EventArgs.Empty);
        }

        private static void ActualizarBotonHabilitado(object? sender, EventArgs e)
        {
            if (sender is not Button boton)
            {
                return;
            }
            boton.ForeColor = boton.Enabled
                ? (boton.BackColor == Superficie ? Texto : Color.White)
                : TextoSecundario;
        }

        private static void EstilarGrilla(DataGridView grilla)
        {
            grilla.BackgroundColor = Superficie;
            grilla.BorderStyle = BorderStyle.None;
            grilla.GridColor = Borde;
            grilla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grilla.EnableHeadersVisualStyles = false;
            grilla.RowHeadersVisible = false;
            grilla.Font = FuenteNormal;

            grilla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grilla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grilla.ColumnHeadersHeight = 36;
            grilla.ColumnHeadersDefaultCellStyle.BackColor = EncabezadoGrilla;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = Texto;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = EncabezadoGrilla;
            grilla.ColumnHeadersDefaultCellStyle.Font = FuenteNegrita;
            grilla.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
            grilla.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            grilla.RowTemplate.Height = 32;
            grilla.DefaultCellStyle.BackColor = Superficie;
            grilla.DefaultCellStyle.ForeColor = Texto;
            grilla.DefaultCellStyle.SelectionBackColor = Seleccion;
            grilla.DefaultCellStyle.SelectionForeColor = Texto;
            grilla.DefaultCellStyle.Padding = new Padding(4, 0, 0, 0);

            foreach (ScrollBar barra in grilla.Controls.OfType<ScrollBar>())
            {
                UsarTemaOscuroDeWindows(barra, "DarkMode_Explorer");
            }
        }

        // Las flechas de los campos numéricos son nativas y siempre claras: se redibujan con los colores del tema.
        private static void EstilarCampoNumerico(NumericUpDown numero)
        {
            numero.BackColor = Superficie;
            numero.ForeColor = Texto;
            numero.BorderStyle = BorderStyle.FixedSingle;
            numero.Font = FuenteCampo;

            foreach (Control parte in numero.Controls)
            {
                parte.BackColor = Superficie;
                parte.ForeColor = Texto;
                if (parte is not TextBox)
                {
                    parte.Paint -= PintarFlechas;
                    parte.Paint += PintarFlechas;
                    parte.MouseMove -= Repintar;
                    parte.MouseMove += Repintar;
                    parte.MouseUp -= Repintar;
                    parte.MouseUp += Repintar;
                    parte.MouseLeave -= Repintar;
                    parte.MouseLeave += Repintar;
                }
            }
        }

        private static void Repintar(object? sender, EventArgs e)
        {
            (sender as Control)?.Invalidate();
        }

        private static void PintarFlechas(object? sender, PaintEventArgs e)
        {
            if (sender is not Control flechas)
            {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle area = flechas.ClientRectangle;
            int mitad = area.Height / 2;
            Point puntero = flechas.PointToClient(Cursor.Position);
            g.Clear(Superficie);

            if (area.Contains(puntero))
            {
                using (SolidBrush resaltado = new SolidBrush(Seleccion))
                {
                    g.FillRectangle(resaltado, 0, puntero.Y < mitad ? 0 : mitad, area.Width, puntero.Y < mitad ? mitad : area.Height - mitad);
                }
            }
            using (Pen borde = new Pen(Borde))
            {
                g.DrawLine(borde, 0, 0, 0, area.Height);
                g.DrawLine(borde, 0, mitad, area.Width, mitad);
            }

            bool habilitado = flechas.Parent?.Enabled ?? true;
            using (SolidBrush pincel = new SolidBrush(habilitado ? Texto : TextoSecundario))
            {
                float centroX = area.Width / 2f;
                float arriba = mitad / 2f;
                float abajo = mitad + (area.Height - mitad) / 2f;
                g.FillPolygon(pincel, new[] { new PointF(centroX - 3.5f, arriba + 1.5f), new PointF(centroX + 3.5f, arriba + 1.5f), new PointF(centroX, arriba - 1.5f) });
                g.FillPolygon(pincel, new[] { new PointF(centroX - 3.5f, abajo - 1.5f), new PointF(centroX + 3.5f, abajo - 1.5f), new PointF(centroX, abajo + 1.5f) });
            }
        }

        // Windows pinta en blanco la parte visible de los desplegables: sus elementos se dibujan con los colores del tema.
        private static void EstilarDesplegable(ComboBox combo)
        {
            combo.BackColor = Superficie;
            combo.ForeColor = Texto;
            combo.Font = FuenteCampo;
            combo.FlatStyle = FlatStyle.Standard;
            combo.DrawMode = DrawMode.OwnerDrawFixed;
            UsarTemaOscuroDeWindows(combo, "DarkMode_CFD");
            combo.DrawItem -= DibujarElementoDesplegable;
            combo.DrawItem += DibujarElementoDesplegable;
        }

        private static void DibujarElementoDesplegable(object? sender, DrawItemEventArgs e)
        {
            if (sender is not ComboBox combo)
            {
                return;
            }
            bool resaltado = (e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.ComboBoxEdit) == 0;
            using (SolidBrush fondo = new SolidBrush(resaltado ? Seleccion : Superficie))
            {
                e.Graphics.FillRectangle(fondo, e.Bounds);
            }
            if (e.Index >= 0)
            {
                TextRenderer.DrawText(e.Graphics, combo.GetItemText(combo.Items[e.Index]), combo.Font, e.Bounds,
                    combo.Enabled ? Texto : TextoSecundario,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        // Línea a la izquierda del botón del menú de la pantalla abierta.
        private static void DibujarMarcaActivo(object? sender, PaintEventArgs e)
        {
            if (sender is Button boton && "activo".Equals(boton.Tag))
            {
                using (SolidBrush marca = new SolidBrush(Acento))
                {
                    e.Graphics.FillRectangle(marca, 0, 6, 4, boton.Height - 12);
                }
            }
        }

        // Pide a Windows (10 20H1 en adelante) la barra de título oscura; en versiones viejas no hace nada.
        private static void UsarBarraDeTituloOscura(Form? form)
        {
            if (form == null)
            {
                return;
            }
            if (form.IsHandleCreated)
            {
                ActivarBarraOscura(form);
            }
            else
            {
                form.HandleCreated += (sender, e) => ActivarBarraOscura(form);
            }
        }

        private static void ActivarBarraOscura(Form form)
        {
            int activado = 1;
            DwmSetWindowAttribute(form.Handle, 20, ref activado, sizeof(int));
        }

        // Estilo oscuro de Windows 10/11 para controles nativos (barras de desplazamiento, flecha de los desplegables).
        private static void UsarTemaOscuroDeWindows(Control control, string tema)
        {
            if (control.IsHandleCreated)
            {
                SetWindowTheme(control.Handle, tema, null);
            }
            else
            {
                control.HandleCreated += (sender, e) => SetWindowTheme(control.Handle, tema, null);
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int atributo, ref int valor, int tamanio);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string? subAppName, string? subIdList);
    }
}
