namespace CooperadoraISFT94.Forms.Utils
{
    internal static class Navegacion
    {
        // Abre una pantalla dentro de la ventana principal (con Volver a la anterior). Si la pantalla de origen no está
        // dentro de la ventana principal, la abre como ventana aparte.
        public static void AbrirSubpantalla(Form origen, Form pantalla)
        {
            if (origen.ParentForm is FrmPrincipal principal)
            {
                principal.AbrirSubpantalla(pantalla);
                return;
            }

            using (pantalla)
            {
                pantalla.StartPosition = FormStartPosition.CenterParent;
                pantalla.ShowDialog(origen);
            }
            if (origen is IVuelveDeSubpantalla destino)
            {
                destino.AlVolverDeSubpantalla(pantalla);
            }
        }
    }
}
