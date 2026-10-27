using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Forms.Utils
{
    // Avisos que necesitan respuesta del usuario. Usan FrmMensaje (con los colores del tema) en lugar de MessageBox.
    // Para avisos breves que no interrumpen (por ejemplo, "Socio guardado.") se usa Aviso.
    internal static class Mensajes
    {
        public static void Advertencia(string texto)
        {
            FrmMensaje.Mostrar("Atención", texto, false);
        }

        public static void Informacion(string texto)
        {
            FrmMensaje.Mostrar("Información", texto, false);
        }

        public static bool Confirmar(string texto)
        {
            return FrmMensaje.Mostrar("Confirmar", texto, true);
        }

        // Errores de programación: se guardan en el registro de errores y se muestran. Si el aviso propio
        // también falla, se usa el de Windows.
        public static void ErrorInesperado(Exception excepcion)
        {
            RegistroErrores.Registrar(excepcion);
            string texto = "Ocurrió un error inesperado: " + excepcion.Message
                + "\n\nQuedó registrado en " + Configuracion.RutaRegistroErrores + ".";
            try
            {
                FrmMensaje.Mostrar("Error", texto, false);
            }
            catch (Exception)
            {
                MessageBox.Show(texto, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
