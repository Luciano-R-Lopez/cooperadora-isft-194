using System.ComponentModel;
using System.Globalization;

namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>
    /// Campo de fecha con los colores del <see cref="Tema"/>: el DateTimePicker de Windows (y su calendario)
    /// siempre se ve claro. Se puede escribir la fecha (dd/mm/aaaa) o elegirla en el calendario desplegable.
    /// </summary>
    public class CampoFecha : UserControl
    {
        private const string Formato = "dd/MM/yyyy";

        private readonly TextBox _texto = new() { BorderStyle = BorderStyle.FixedSingle };
        private readonly Button _boton = new() { Text = "▾", Width = 26, FlatStyle = FlatStyle.Flat };
        private DateTime _valor = DateTime.Today;
        private ToolStripDropDown? _desplegable;

        /// <summary>Se dispara cuando cambia la fecha (escrita o elegida).</summary>
        public event EventHandler? ValueChanged;

        public CampoFecha()
        {
            Height = 27;
            Width = 130;
            Tag = Tema.ColoresPropios;
            BackColor = Tema.Superficie;

            _texto.BackColor = Tema.Superficie;
            _texto.ForeColor = Tema.Texto;
            _texto.Dock = DockStyle.Fill;
            _texto.Text = _valor.ToString(Formato);
            _texto.Leave += (_, _) => TomarTextoEscrito();
            _texto.KeyDown += (_, e) =>
            {
                if (e.KeyCode != Keys.Enter) return;
                TomarTextoEscrito();
                e.SuppressKeyPress = true;
            };

            _boton.Tag = Tema.ColoresPropios;
            _boton.Dock = DockStyle.Right;
            _boton.BackColor = Tema.Superficie;
            _boton.ForeColor = Tema.Texto;
            _boton.FlatAppearance.BorderColor = Tema.Borde;
            _boton.FlatAppearance.MouseOverBackColor = Tema.Seleccion;
            _boton.Cursor = Cursors.Hand;
            _boton.Click += (_, _) => MostrarCalendario();

            Controls.Add(_texto);
            Controls.Add(_boton);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime Value
        {
            get => _valor;
            set
            {
                var fecha = value.Date;
                _texto.Text = fecha.ToString(Formato);
                if (fecha == _valor) return;
                _valor = fecha;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _texto.ForeColor = Enabled ? Tema.Texto : Tema.TextoSecundario;
            _boton.ForeColor = Enabled ? Tema.Texto : Tema.TextoSecundario;
        }

        /// <summary>Si lo escrito es una fecha válida la toma; si no, vuelve a mostrar la anterior.</summary>
        private void TomarTextoEscrito()
        {
            if (DateTime.TryParseExact(_texto.Text.Trim(), new[] { Formato, "d/M/yyyy" }, CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out var fecha))
                Value = fecha;
            else
                _texto.Text = _valor.ToString(Formato);
        }

        // El desplegable anterior se libera recién al abrir otro (o al liberar el campo): liberarlo en su
        // evento Closed rompe a WinForms, que sigue usándolo después de cerrarlo (ObjectDisposedException).
        private void MostrarCalendario()
        {
            _desplegable?.Dispose();

            var calendario = new Calendario(_valor);
            var anfitrion = new ToolStripControlHost(calendario) { Margin = Padding.Empty, Padding = Padding.Empty, AutoSize = false, Size = calendario.Size };
            var desplegable = new ToolStripDropDown { Padding = Padding.Empty, BackColor = Tema.Borde };
            desplegable.Items.Add(anfitrion);

            calendario.FechaElegida += fecha =>
            {
                desplegable.Close();
                Value = fecha;
            };
            _desplegable = desplegable;
            desplegable.Show(this, new Point(0, Height));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _desplegable?.Dispose();
                _desplegable = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>Mes con sus días como botones; las flechas cambian de mes.</summary>
        private sealed class Calendario : Panel
        {
            private const int AnchoCelda = 34;
            private const int AltoCelda = 28;
            private static readonly string[] Dias = { "lu", "ma", "mi", "ju", "vi", "sá", "do" };

            private readonly DateTime _elegida;
            private readonly Label _titulo = new() { TextAlign = ContentAlignment.MiddleCenter, ForeColor = Tema.Texto };
            private readonly Button[] _celdas = new Button[42];
            private DateTime _mes;

            public event Action<DateTime>? FechaElegida;

            public Calendario(DateTime elegida)
            {
                _elegida = elegida.Date;
                _mes = new DateTime(elegida.Year, elegida.Month, 1);

                BackColor = Tema.Superficie;
                Size = new Size(AnchoCelda * 7 + 2, 30 + 22 + AltoCelda * 6 + 4);

                var anterior = CrearBoton("◀", new Rectangle(2, 2, 30, 26));
                anterior.Click += (_, _) => CambiarMes(-1);
                var siguiente = CrearBoton("▶", new Rectangle(Width - 32, 2, 30, 26));
                siguiente.Click += (_, _) => CambiarMes(1);

                _titulo.Bounds = new Rectangle(34, 2, Width - 68, 26);
                _titulo.Font = new Font(Font, FontStyle.Bold);
                Controls.AddRange(new Control[] { anterior, siguiente, _titulo });

                for (var i = 0; i < Dias.Length; i++)
                {
                    Controls.Add(new Label
                    {
                        Text = Dias[i],
                        ForeColor = Tema.TextoSecundario,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Bounds = new Rectangle(1 + i * AnchoCelda, 30, AnchoCelda, 22)
                    });
                }

                for (var i = 0; i < _celdas.Length; i++)
                {
                    var celda = CrearBoton(string.Empty,
                        new Rectangle(1 + i % 7 * AnchoCelda, 52 + i / 7 * AltoCelda, AnchoCelda, AltoCelda));
                    celda.Click += (_, _) => { if (celda.Tag is DateTime fecha) FechaElegida?.Invoke(fecha); };
                    _celdas[i] = celda;
                    Controls.Add(celda);
                }

                MostrarMes();
            }

            private static Button CrearBoton(string texto, Rectangle limites)
            {
                var boton = new Button
                {
                    Text = texto,
                    Bounds = limites,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Tema.Superficie,
                    ForeColor = Tema.Texto,
                    Cursor = Cursors.Hand,
                    TabStop = false
                };
                boton.FlatAppearance.BorderSize = 0;
                boton.FlatAppearance.MouseOverBackColor = Tema.Seleccion;
                return boton;
            }

            private void CambiarMes(int meses)
            {
                _mes = _mes.AddMonths(meses);
                MostrarMes();
            }

            private void MostrarMes()
            {
                _titulo.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_mes.ToString("MMMM yyyy"));

                // La semana empieza el lunes.
                var desfase = ((int)_mes.DayOfWeek + 6) % 7;
                var diasDelMes = DateTime.DaysInMonth(_mes.Year, _mes.Month);

                for (var i = 0; i < _celdas.Length; i++)
                {
                    var celda = _celdas[i];
                    var dia = i - desfase + 1;
                    var visible = dia >= 1 && dia <= diasDelMes;
                    celda.Visible = visible;
                    if (!visible) continue;

                    var fecha = new DateTime(_mes.Year, _mes.Month, dia);
                    celda.Tag = fecha;
                    celda.Text = dia.ToString();

                    var esElegida = fecha == _elegida;
                    celda.BackColor = esElegida ? Tema.Primario : Tema.Superficie;
                    celda.ForeColor = esElegida ? Color.White : Tema.Texto;
                    celda.FlatAppearance.BorderSize = fecha == DateTime.Today && !esElegida ? 1 : 0;
                    celda.FlatAppearance.BorderColor = Tema.Primario;
                }
            }
        }
    }
}
