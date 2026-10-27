namespace CooperadoraISFT94.Forms.Utils
{
    // Detecta si quedaron datos escritos sin guardar: se toma una "foto" de los valores al limpiar o cargar
    // el formulario y se compara con los valores actuales.
    internal static class Formulario
    {
        // Un panel con este Tag queda afuera de la detección de cambios (no es parte de lo que se guarda).
        public const string NoEsDelFormulario = "NoEsDelFormulario";

        public static string Valores(Control contenedor)
        {
            List<string> valores = new List<string>();
            AgregarValores(contenedor, valores);
            return string.Join("|", valores);
        }

        private static void AgregarValores(Control contenedor, List<string> valores)
        {
            foreach (Control control in contenedor.Controls)
            {
                if (NoEsDelFormulario.Equals(control.Tag))
                {
                    continue;
                }
                switch (control)
                {
                    case TextBox texto:
                        valores.Add(texto.Text.Trim());
                        break;
                    case NumericUpDown numero:
                        valores.Add(numero.Value.ToString());
                        break;
                    case ComboBox combo:
                        valores.Add(combo.SelectedIndex.ToString());
                        break;
                    case CheckedListBox lista:
                        valores.Add(string.Join(",", lista.CheckedIndices.Cast<int>()));
                        break;
                    case CheckBox check:
                        valores.Add(check.Checked.ToString());
                        break;
                    case CampoFecha fecha:
                        valores.Add(fecha.Value.ToString("d"));
                        break;
                    default:
                        AgregarValores(control, valores);
                        break;
                }
            }
        }

        public static bool ConfirmarDescartarCambios(bool hayCambios)
        {
            return !hayCambios || Mensajes.Confirmar("Hay datos sin guardar en el formulario. ¿Desea descartarlos?");
        }
    }
}
