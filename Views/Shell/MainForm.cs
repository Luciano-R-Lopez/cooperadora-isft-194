using CooperadoraISFT94.Common;
using CooperadoraISFT94.Controllers;
using CooperadoraISFT94.DTOs.Usuarios;
using CooperadoraISFT94.Views.Common;
using CooperadoraISFT94.Views.Modulos;

namespace CooperadoraISFT94.Views.Shell
{
    /// <summary>
    /// Ventana principal. Muestra una pestaña por cada módulo que corresponde al rol del usuario logueado
    /// (ver <see cref="ModulosPorRol"/>).
    /// </summary>
    public class MainForm : Form
    {
        private readonly AuthController _auth = new();
        private readonly UsuarioDto _usuario;
        private readonly Label _lblUsuario = new() { AutoSize = true, Location = new Point(12, 14) };

        /// <summary>True si el usuario eligió "Cerrar sesión" (volver al login) en lugar de cerrar la app.</summary>
        public bool CerroSesion { get; private set; }

        public MainForm()
        {
            _usuario = SesionActual.Instancia.Usuario
                ?? throw new InvalidOperationException("No hay una sesión iniciada.");
            ConstruirInterfaz();
            Tema.Aplicar(this);

            // Si el usuario edita sus propios datos, la barra superior los refleja sin volver a iniciar sesión.
            SesionActual.Instancia.UsuarioActualizado += MostrarUsuario;
            FormClosed += (_, _) => SesionActual.Instancia.UsuarioActualizado -= MostrarUsuario;
        }

        private void ConstruirInterfaz()
        {
            Text = "Cooperadora ISFT 94";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 650);
            MinimumSize = new Size(800, 500);
            WindowState = FormWindowState.Maximized;

            var barra = Tema.ConColoresPropios(new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Tema.Encabezado });
            Tema.ConColoresPropios(_lblUsuario);
            _lblUsuario.ForeColor = Tema.EncabezadoTexto;

            MostrarUsuario(_usuario);

            var btnCerrarSesion = Tema.Secundario(new Button
            {
                Text = "Cerrar sesión",
                Size = new Size(120, 30),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(barra.Width - 132, 7)
            });
            btnCerrarSesion.Click += (_, _) => CerrarSesion();

            barra.Controls.Add(_lblUsuario);
            barra.Controls.Add(btnCerrarSesion);

            var tabs = new Pestanias { Dock = DockStyle.Fill };

            var modulos = ModulosPorRol.De(_usuario.Rol);
            if (modulos.Count == 0)
                AgregarTab(tabs, _usuario.RolTexto, new ModuloPlaceholderView(_usuario.RolTexto));
            foreach (var modulo in modulos)
                AgregarTab(tabs, modulo.Titulo, modulo.CrearVista());

            // Al cambiar de pestaña se recargan los datos de la vista que lo soporte.
            tabs.SelectedIndexChanged += (_, _) =>
                tabs.SelectedTab?.Controls.OfType<IRefrescable>().FirstOrDefault()?.Refrescar();

            // Orden de agregado: primero el que llena, después la barra superior.
            Controls.Add(tabs);
            Controls.Add(barra);
        }

        private void MostrarUsuario(UsuarioDto usuario) =>
            _lblUsuario.Text = $"{usuario.NombreCompleto}  ({usuario.RolTexto})";

        private static void AgregarTab(TabControl tabs, string titulo, Control vista)
        {
            vista.Dock = DockStyle.Fill;
            var pagina = new TabPage(titulo);
            pagina.Controls.Add(vista);
            tabs.TabPages.Add(pagina);
        }

        private void CerrarSesion()
        {
            _auth.CerrarSesion();
            CerroSesion = true;
            Close();
        }
    }
}
