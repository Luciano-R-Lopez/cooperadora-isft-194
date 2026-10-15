using CooperadoraISFT94.Views.Common;

namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Contenedor del módulo Contador: caja, egresos y movimientos.</summary>
    public class ContadorView : UserControl
    {
        private readonly TabControl _tabs = new Pestanias { Dock = DockStyle.Fill };

        public ContadorView()
        {
            Dock = DockStyle.Fill;

            AgregarTab("Caja", new CajaView());
            AgregarTab("Egresos", new EgresoView());
            AgregarTab("Movimientos", new MovimientosView());

            // Al cambiar de pestaña se recargan los datos, para reflejar lo registrado en las otras.
            _tabs.SelectedIndexChanged += (_, _) => RefrescarTabActual();

            Controls.Add(_tabs);
        }

        private void AgregarTab(string titulo, Control vista)
        {
            vista.Dock = DockStyle.Fill;
            var pagina = new TabPage(titulo);
            pagina.Controls.Add(vista);
            _tabs.TabPages.Add(pagina);
        }

        private void RefrescarTabActual() =>
            _tabs.SelectedTab?.Controls.OfType<IRefrescable>().FirstOrDefault()?.Refrescar();
    }
}
