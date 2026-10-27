namespace CooperadoraISFT94.Forms.Modulos
{
    /// <summary>Pantalla provisoria de cada módulo. Se reemplaza por la vista real de cada rol.</summary>
    public class ModuloPlaceholderView : UserControl
    {
        public ModuloPlaceholderView(string titulo)
        {
            Dock = DockStyle.Fill;
            Controls.Add(new Label
            {
                Text = $"{titulo}\n\n(próximamente)",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font(Font.FontFamily, 14f)
            });
        }
    }
}
