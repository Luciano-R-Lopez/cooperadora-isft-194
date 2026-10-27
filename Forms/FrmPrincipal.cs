using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Controller;
using CooperadoraISFT94.Dto;
using CooperadoraISFT94.Forms.Utils;

namespace CooperadoraISFT94.Forms
{
    // Ventana principal: menú lateral con las secciones de los roles del usuario (un usuario con varios roles ve las
    // de todos) y cada sección se abre dentro de la misma ventana.
    public partial class FrmPrincipal : Form
    {
        private Form? pantallaActual;
        private Button? botonActivo;
        private bool descartando;

        private readonly Stack<(Form Pantalla, string Titulo)> historial = new Stack<(Form Pantalla, string Titulo)>();

        public FrmPrincipal()
        {
            InitializeComponent();
            Tema.EstilarMenu(pnlMenu, lblTitulo, lblUsuario, pnlEncabezado, lblPantalla);
            BackColor = Tema.Fondo;
            pnlContenido.BackColor = Tema.Fondo;
            foreach (Label seccion in new[] { lblSeccionAdministrador, lblSeccionOperador, lblSeccionContador })
            {
                seccion.ForeColor = Tema.TextoSecundario;
                seccion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            }
        }

        // True si se cerró con "Cerrar sesión" (vuelve a la pantalla de ingreso); false si se salió de la aplicación.
        public bool CerroSesion { get; private set; }

        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            MostrarUsuario(SesionActual.Instancia.Usuario);
            SesionActual.Instancia.UsuarioActualizado += MostrarUsuario;
            ConfigurarMenu();
            AbrirEnPanel(new FrmInicio(), btnInicio);
        }

        private void ConfigurarMenu()
        {
            SesionActual sesion = SesionActual.Instancia;
            bool administrador = sesion.TieneRol(Rol.Administrador);
            bool operador = sesion.TieneRol(Rol.Operador);
            bool contador = sesion.TieneRol(Rol.Contador);

            lblSeccionAdministrador.Visible = administrador;
            btnUsuarios.Visible = administrador;
            btnCatalogos.Visible = administrador;
            btnAuditoria.Visible = administrador;

            lblSeccionOperador.Visible = operador;
            btnSocios.Visible = operador;
            btnCuotas.Visible = operador;
            btnIngresos.Visible = operador;

            lblSeccionContador.Visible = contador;
            btnCaja.Visible = contador;
            btnEgresos.Visible = contador;
            btnMovimientos.Visible = contador;
        }

        private void MostrarUsuario(UsuarioDTO? usuario)
        {
            if (usuario == null)
            {
                return;
            }
            lblUsuario.Text = usuario.NombreCompleto + Environment.NewLine + usuario.RolesTexto;
        }

        private void AbrirEnPanel(Form pantalla, Button boton)
        {
            if (!CerrarPantallas())
            {
                pantalla.Dispose();
                return;
            }

            // Desde el menú no hay a dónde volver.
            foreach (Control volver in pantalla.Controls.Find("btnVolver", true))
            {
                volver.Visible = false;
            }
            Incrustar(pantalla);

            lblPantalla.Text = pantalla.Text;
            MarcarBotonActivo(boton);
        }

        public void AbrirSubpantalla(Form subpantalla)
        {
            Form anterior = pantallaActual!;
            historial.Push((anterior, lblPantalla.Text));
            anterior.Visible = false;

            subpantalla.FormClosed += (sender, e) => VolverAPantallaAnterior(subpantalla);
            subpantalla.TextChanged += (sender, e) =>
            {
                if (pantallaActual == subpantalla)
                {
                    lblPantalla.Text = subpantalla.Text;
                }
            };

            Incrustar(subpantalla);
            lblPantalla.Text = subpantalla.Text;
        }

        // Cierra la pantalla abierta y las que quedaron atrás. Devuelve false si el usuario decidió no descartar
        // datos sin guardar.
        private bool CerrarPantallas()
        {
            descartando = true;
            if (pantallaActual != null && !IntentarCerrar(pantallaActual))
            {
                descartando = false;
                return false;
            }
            pantallaActual?.Dispose();
            while (historial.Count > 0)
            {
                Form oculta = historial.Pop().Pantalla;
                oculta.Close();
                oculta.Dispose();
            }
            pantallaActual = null;
            descartando = false;
            return true;
        }

        private void Incrustar(Form pantalla)
        {
            pantalla.TopLevel = false;
            pantalla.FormBorderStyle = FormBorderStyle.None;
            pantalla.Dock = DockStyle.Fill;
            pantallaActual = pantalla;

            pnlContenido.Controls.Add(pantalla);
            pantalla.Show();

            QuitarSeleccionGrillas(pantalla);
            BeginInvoke(() => QuitarSeleccionGrillas(pantalla));
        }

        private void VolverAPantallaAnterior(Form subpantalla)
        {
            if (descartando || historial.Count == 0 || pantallaActual != subpantalla)
            {
                return;
            }
            (Form Pantalla, string Titulo) previo = historial.Pop();
            pnlContenido.Controls.Remove(subpantalla);
            subpantalla.Dispose();
            pantallaActual = previo.Pantalla;
            previo.Pantalla.Visible = true;
            lblPantalla.Text = previo.Titulo;
            if (previo.Pantalla is IVuelveDeSubpantalla destino)
            {
                destino.AlVolverDeSubpantalla(subpantalla);
            }
        }

        private static bool IntentarCerrar(Form pantalla)
        {
            bool cerrada = false;
            FormClosedEventHandler alCerrar = (sender, e) => cerrada = true;
            pantalla.FormClosed += alCerrar;
            pantalla.Close();
            pantalla.FormClosed -= alCerrar;
            return cerrada;
        }

        private static void QuitarSeleccionGrillas(Control contenedor)
        {
            if (contenedor.IsDisposed)
            {
                return;
            }
            foreach (Control control in contenedor.Controls)
            {
                if (control is DataGridView grilla)
                {
                    grilla.QuitarSeleccion();
                }
                else
                {
                    QuitarSeleccionGrillas(control);
                }
            }
        }

        private void MarcarBotonActivo(Button boton)
        {
            if (botonActivo != null)
            {
                Tema.MarcarBotonMenu(botonActivo, false);
            }
            botonActivo = boton;
            Tema.MarcarBotonMenu(botonActivo, true);
        }

        private void FrmPrincipal_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!CerrarPantallas())
            {
                e.Cancel = true;
                return;
            }
            SesionActual.Instancia.UsuarioActualizado -= MostrarUsuario;
        }

        private void btnInicio_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmInicio(), btnInicio);
        }

        private void btnUsuarios_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmUsuarios(), btnUsuarios);
        }

        private void btnCatalogos_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmCatalogos(), btnCatalogos);
        }

        private void btnAuditoria_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmAuditoria(), btnAuditoria);
        }

        private void btnSocios_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmSocios(), btnSocios);
        }

        private void btnCuotas_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmCuotas(), btnCuotas);
        }

        private void btnIngresos_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmIngresos(), btnIngresos);
        }

        private void btnCaja_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmCaja(), btnCaja);
        }

        private void btnEgresos_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmEgresos(), btnEgresos);
        }

        private void btnMovimientos_Click(object? sender, EventArgs e)
        {
            AbrirEnPanel(new FrmMovimientos(), btnMovimientos);
        }

        private void btnMiPassword_Click(object? sender, EventArgs e)
        {
            using (FrmCambiarPassword dialogo = new FrmCambiarPassword())
            {
                dialogo.ShowDialog(this);
            }
        }

        private void btnCerrarSesion_Click(object? sender, EventArgs e)
        {
            if (!Mensajes.Confirmar("¿Cerrar la sesión de " + SesionActual.Instancia.Usuario?.NombreCompleto + "?"))
            {
                return;
            }
            if (!CerrarPantallas())
            {
                return;
            }
            new AuthController().CerrarSesion();
            CerroSesion = true;
            Close();
        }

        private void btnSalir_Click(object? sender, EventArgs e)
        {
            if (Mensajes.Confirmar("¿Seguro que desea salir?"))
            {
                Close();
            }
        }
    }
}
