using System.Globalization;
using CooperadoraISFT94.Common;
using CooperadoraISFT94.Repositories.InMemory;
using CooperadoraISFT94.Views.Auth;
using CooperadoraISFT94.Views.Common;
using CooperadoraISFT94.Views.Shell;

namespace CooperadoraISFT94
{
    internal static class Program
    {
        [STAThread]
        static void Main()
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

            // Datos de prueba en memoria (se elimina al conectar MySQL).
            DatosIniciales.Cargar();

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
