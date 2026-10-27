using System.Globalization;
using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Datos;
using CooperadoraISFT94.Forms;
using CooperadoraISFT94.Forms.Utils;
using CooperadoraISFT94.Repository.MySql;

namespace CooperadoraISFT94
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Montos y fechas siempre con formato argentino ("$ 5.000,00"), sin depender de la configuración de Windows.
            CultureInfo cultura = new CultureInfo("es-AR");
            CultureInfo.DefaultThreadCurrentCulture = cultura;
            CultureInfo.DefaultThreadCurrentUICulture = cultura;
            CultureInfo.CurrentCulture = cultura;
            CultureInfo.CurrentUICulture = cultura;

            ApplicationConfiguration.Initialize();

            // Los errores de negocio (NegocioException) los atrapa cada pantalla; cualquier otra excepción es un error
            // de programación: se muestra y queda registrada en lugar de cerrar la aplicación sin aviso.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => Mensajes.ErrorInesperado(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception excepcion)
                {
                    RegistroErrores.Registrar(excepcion);
                }
            };

            if (!CargarDatos())
            {
                return;
            }

            bool volverAlLogin;
            do
            {
                using (FrmLogin login = new FrmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                }
                using (FrmPrincipal principal = new FrmPrincipal())
                {
                    Application.Run(principal);
                    volverAlLogin = principal.CerroSesion;
                }
            }
            while (volverAlLogin);
        }

        // Con base de datos se lee todo de MySQL (los catálogos y usuarios iniciales los trae el script).
        // Sin base: catálogos y usuarios en memoria y, en modo Debug, además datos de ejemplo.
        private static bool CargarDatos()
        {
            if (!Configuracion.UsarBaseDeDatos)
            {
                DatosIniciales.Cargar();
#if DEBUG
                DatosDeEjemplo.Cargar();
#endif
                return true;
            }

            try
            {
                CargadorMySql.Cargar();
                return true;
            }
            catch (NegocioException ex)
            {
                Mensajes.Advertencia(ex.Message);
                return false;
            }
        }
    }
}
