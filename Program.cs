using CooperadoraISFT94.Repositories.MySql;
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

            // Los errores de negocio (NegocioException) los atrapa cada vista; cualquier otra excepción
            // es un error de programación y se informa acá en lugar de cerrar la aplicación sin aviso.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => Ui.ErrorInesperado(e.Exception);

            // Conecta con MySQL (XAMPP), carga los datos y desde ahí guarda cada cambio en la base.
            try
            {
                BaseDatos.Inicializar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo conectar con la base de datos.\n\n" +
                    "Revisá que MySQL esté iniciado en XAMPP, que exista la base 'cooperadora' y que el script SQL esté importado.\n\n" +
                    ex.Message,
                    "Cooperadora ISFT 94", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
