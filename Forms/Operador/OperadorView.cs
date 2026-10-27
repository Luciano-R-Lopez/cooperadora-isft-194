namespace CooperadoraISFT94.Forms.Operador
{
    /// <summary>Contenedor del módulo Operador: navega entre el listado de socios y la ficha de un socio.</summary>
    public class OperadorView : UserControl
    {
        public OperadorView()
        {
            Dock = DockStyle.Fill;
            MostrarSocios();
        }

        private void MostrarSocios()
        {
            var vista = new SociosView();
            vista.VerCuotasSolicitado += MostrarFicha;
            Mostrar(vista);
        }

        private void MostrarFicha(int socioId)
        {
            var vista = new FichaSocioView(socioId);
            vista.VolverSolicitado += MostrarSocios;
            Mostrar(vista);
        }

        private void Mostrar(Control vista)
        {
            var anteriores = Controls.Cast<Control>().ToList();

            vista.Dock = DockStyle.Fill;
            Controls.Add(vista);

            foreach (var anterior in anteriores)
            {
                Controls.Remove(anterior);
                // Se libera después de que termine el evento que provocó el cambio de pantalla.
                BeginInvoke(new Action(anterior.Dispose));
            }
        }
    }
}
