using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Domain
{
    // Datos comunes a usuarios y socios.
    public abstract class Persona : IEntidad
    {
        public const int LargoMaximoNombre = 50;

        private string nombre = string.Empty;
        private string apellido = string.Empty;

        protected Persona(int id, string nombre, string apellido)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
        }

        public int Id { get; private set; }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = Validaciones.ExigirTexto(value, "nombre", LargoMaximoNombre); }
        }

        public string Apellido
        {
            get { return apellido; }
            set { apellido = Validaciones.ExigirTexto(value, "apellido", LargoMaximoNombre); }
        }

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }
    }
}
