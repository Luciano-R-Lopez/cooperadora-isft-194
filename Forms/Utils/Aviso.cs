namespace CooperadoraISFT94.Forms.Utils
{
    // Aviso breve en una etiqueta del formulario ("Socio guardado."), que se borra solo después de unos segundos.
    internal static class Aviso
    {
        private const int DuracionMilisegundos = 4000;

        public static void Mostrar(Label etiqueta, string texto)
        {
            if (etiqueta.Tag is System.Windows.Forms.Timer anterior)
            {
                anterior.Stop();
                anterior.Dispose();
            }

            etiqueta.Text = texto;
            System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
            temporizador.Interval = DuracionMilisegundos;
            temporizador.Tick += (sender, e) =>
            {
                temporizador.Stop();
                temporizador.Dispose();
                etiqueta.Tag = null;
                if (!etiqueta.IsDisposed)
                {
                    etiqueta.Text = string.Empty;
                }
            };
            etiqueta.Tag = temporizador;
            temporizador.Start();
        }
    }
}
