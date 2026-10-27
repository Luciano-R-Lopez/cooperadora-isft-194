namespace CooperadoraISFT94.Forms.Utils
{
    /// <summary>
    /// TabControl dibujado con los colores del <see cref="Tema"/>: el de Windows siempre pinta
    /// las pestañas en claro, que desentona con el tema oscuro.
    /// </summary>
    public class Pestanias : TabControl
    {
        private const int AltoAcento = 2;

        public Pestanias()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(160, 34);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Tema.Fondo);

            for (var i = 0; i < TabPages.Count; i++)
            {
                var area = GetTabRect(i);
                var seleccionada = i == SelectedIndex;

                using (var fondo = new SolidBrush(seleccionada ? Tema.Superficie : Tema.Fondo))
                    e.Graphics.FillRectangle(fondo, area);

                // La pestaña elegida se marca con una línea del color de las acciones.
                if (seleccionada)
                {
                    using var acento = new SolidBrush(Tema.Primario);
                    e.Graphics.FillRectangle(acento, area.Left, area.Bottom - AltoAcento, area.Width, AltoAcento);
                }

                TextRenderer.DrawText(e.Graphics, TabPages[i].Text, Font, area,
                    seleccionada ? Tema.Texto : Tema.TextoSecundario,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }
    }
}
