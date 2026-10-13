using CooperadoraISFT94.Common;

namespace CooperadoraISFT94.Models
{
    /// <summary>Datos y reglas comunes a las personas del sistema (usuarios y socios).</summary>
    public abstract class Persona : Entidad
    {
        public const int LargoMaximoNombre = 50;

        private string _nombre = string.Empty;
        private string _apellido = string.Empty;

        protected Persona(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }

        public string Nombre
        {
            get => _nombre;
            set => _nombre = Validaciones.ExigirTexto(value, "nombre", LargoMaximoNombre);
        }

        public string Apellido
        {
            get => _apellido;
            set => _apellido = Validaciones.ExigirTexto(value, "apellido", LargoMaximoNombre);
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
