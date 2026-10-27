namespace CooperadoraISFT94.Forms.Utils
{
    // Lo implementa una pantalla que necesita actualizarse cuando se cierra una subpantalla que abrió.
    internal interface IVuelveDeSubpantalla
    {
        void AlVolverDeSubpantalla(Form subpantalla);
    }
}
