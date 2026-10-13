using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Models
{
    /// <summary>Razón por la que sale dinero de la caja. Entidad (no enum) para poder ampliarla sin tocar el código.</summary>
    public class MotivoEgreso : Entidad
    {
        public const int LargoMaximoNombre = 50;

        public MotivoEgreso(string nombre)
        {
            Nombre = Validaciones.ExigirTexto(nombre, "nombre del motivo", LargoMaximoNombre);
        }

        public string Nombre { get; private set; }
    }
}
