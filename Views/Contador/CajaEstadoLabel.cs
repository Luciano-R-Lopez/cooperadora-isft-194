namespace CooperadoraISFT94.Views.Contador
{
    /// <summary>Franja de color que indica si la caja está abierta o cerrada.</summary>
    public class CajaEstadoLabel : Label
    {
        public CajaEstadoLabel()
        {
            Dock = DockStyle.Top;
            Height = 36;
            TextAlign = ContentAlignment.MiddleCenter;
            ForeColor = Color.White;
            Font = new Font(Font.FontFamily, 11f, FontStyle.Bold);
            Mostrar(false);
        }

        public void Mostrar(bool abierta)
        {
            Text = abierta ? "CAJA ABIERTA" : "CAJA CERRADA";
            BackColor = abierta ? Color.SeaGreen : Color.Firebrick;
        }
    }
}
