using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    /// <summary>
    /// Opción configurable (medio de pago, motivo de egreso): son entidades y no enums para poder agregarlas,
    /// renombrarlas o desactivarlas desde la aplicación sin tocar el código.
    /// </summary>
    public abstract class ItemCatalogo : Entidad
    {
        public const int LargoMaximoNombre = 50;

        private readonly string _descripcion;

        /// <param name="descripcion">Cómo se nombra el ítem en los mensajes (por ejemplo, "medio de pago").</param>
        protected ItemCatalogo(string nombre, string descripcion)
        {
            _descripcion = descripcion;
            Nombre = ValidarNombre(nombre);
            Activo = true;
        }

        public string Nombre { get; private set; }

        /// <summary>Los inactivos no se ofrecen para movimientos nuevos; los ya registrados no cambian.</summary>
        public bool Activo { get; private set; }

        public void Renombrar(string nombre) => Nombre = ValidarNombre(nombre);

        public void Activar() => Activo = true;

        public void Desactivar() => Activo = false;

        public void ExigirActivo()
        {
            if (!Activo)
                throw new ReglaNegocioException($"El {_descripcion} \"{Nombre}\" está desactivado.");
        }

        private string ValidarNombre(string nombre) =>
            Validaciones.ExigirTexto(nombre, $"nombre del {_descripcion}", LargoMaximoNombre);
    }
}
