using System.Globalization;
using CooperadoraISFT94.Common;
using CooperadoraISFT94.Repositories.MySql;
using CooperadoraISFT94.Views.Auth;
using CooperadoraISFT94.Views.Common;
using CooperadoraISFT94.Views.Shell;

namespace CooperadoraISFT94
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Montos, fechas y números siempre en formato argentino ("$ 5.000,00"), sin depender de la
            // configuración regional de Windows (por ejemplo, "español (Latinoamérica)" muestra "XDR").
            var argentina = new CultureInfo("es-AR");
            CultureInfo.DefaultThreadCurrentCulture = argentina;
            CultureInfo.DefaultThreadCurrentUICulture = argentina;
            CultureInfo.CurrentCulture = argentina;
            CultureInfo.CurrentUICulture = argentina;

            // Los errores de negocio (NegocioException) los atrapa cada vista; cualquier otra excepción
            // es un error de programación y se informa acá en lugar de cerrar la aplicación sin aviso.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) =>
            {
                RegistroErrores.Registrar(e.Exception);
                Ui.ErrorInesperado(e.Exception);
            };
            // Errores fuera del hilo de la interfaz: la aplicación se cierra, pero queda registrado qué pasó.
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                    RegistroErrores.Registrar(ex);
            };

            // Conecta con MySQL (XAMPP), carga los datos y desde ahí guarda cada cambio en la base.
            try
            {
                BaseDatos.Inicializar();
            }
            catch (Exception ex)
            {
                RegistroErrores.Registrar(ex);
                MessageBox.Show(
                    "No se pudo conectar con la base de datos.\n\n" +
                    "Revisá que MySQL esté iniciado en XAMPP, que exista la base 'cooperadora' y que el script SQL esté importado.\n\n" +
                    ex.Message,
                    "Cooperadora ISFT 194", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool volverAlLogin;
            do
            {
                using (var login = new LoginForm())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                        return;
                }

                using (var principal = new MainForm())
                {
                    Application.Run(principal);
                    volverAlLogin = principal.CerroSesion;
                }
            }
            while (volverAlLogin);
        }
    }
}