using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Models
{
    /// <summary>
    /// Forma en que se cobra o se paga (efectivo, transferencia...). Es una entidad y no un enum
    /// para poder agregar medios nuevos sin tocar el código.
    /// </summary>
    public class MedioPago : Entidad
    {
        public const int LargoMaximoNombre = 50;

        public MedioPago(string nombre)
        {
            Nombre = Validaciones.ExigirTexto(nombre, "nombre del medio de pago", LargoMaximoNombre);
        }

        public string Nombre { get; private set; }
    }
}
