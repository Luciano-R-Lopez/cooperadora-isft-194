using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    // Opción que el administrador mantiene desde la aplicación (medio de pago, motivo o grupo de egreso).
    public abstract class ItemCatalogo : IEntidad
    {
        public const int LargoMaximoNombre = 50;

        private readonly string descripcion;
        private string nombre = string.Empty;

        // descripcion: cómo se nombra el ítem en los mensajes (por ejemplo, "medio de pago").
        protected ItemCatalogo(int id, string nombre, string descripcion)
        {
            this.descripcion = descripcion;
            Id = id;
            Nombre = nombre;
            Activo = true;
        }

        public int Id { get; private set; }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = Validaciones.ExigirTexto(value, "nombre del " + descripcion, LargoMaximoNombre); }
        }

        // Los inactivos no se ofrecen para movimientos nuevos; los ya registrados no cambian.
        public bool Activo { get; set; }

        public void ExigirActivo()
        {
            if (!Activo)
            {
                throw new NegocioException("El " + descripcion + " \"" + Nombre + "\" está desactivado.");
            }
        }
    }
}
