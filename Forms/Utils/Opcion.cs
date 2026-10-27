namespace CooperadoraISFT94.Forms.Utils
{
    // Elemento de un desplegable con un texto para mostrar y un valor (por ejemplo, "Todos" con valor null).
    public class Opcion<T>
    {
        public Opcion(string texto, T valor)
        {
            Texto = texto;
            Valor = valor;
        }

        public string Texto { get; private set; }

        public T Valor { get; private set; }

        public override string ToString()
        {
            return Texto;
        }
    }
}
