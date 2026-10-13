using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Socio : Persona
    {
        private string _dni = string.Empty;
        private string _telefono = string.Empty;
        private string _email = string.Empty;
        private Categoria _categoria;

        public Socio(string nombre, string apellido, string dni, string telefono, string email, Categoria categoria)
            : base(nombre, apellido)
        {
            Dni = dni;
            Telefono = telefono;
            Email = email;
            Categoria = categoria;
        }

        public string Dni
        {
            get => _dni;
            set
            {
                var dni = (value ?? string.Empty).Trim();
                if (!Validaciones.EsDniValido(dni))
                    throw new ReglaNegocioException("El DNI debe tener 7 u 8 dígitos, sin puntos.");
                _dni = dni;
            }
        }

        /// <summary>Opcional: vacío si no se informó.</summary>
        public string Telefono
        {
            get => _telefono;
            set
            {
                var telefono = (value ?? string.Empty).Trim();
                if (telefono.Length > 0 && !Validaciones.EsTelefonoValido(telefono))
                    throw new ReglaNegocioException("El teléfono no es válido.");
                _telefono = telefono;
            }
        }

        /// <summary>Opcional: vacío si no se informó.</summary>
        public string Email
        {
            get => _email;
            set
            {
                var email = (value ?? string.Empty).Trim();
                if (email.Length > 0 && !Validaciones.EsEmailValido(email))
                    throw new ReglaNegocioException("El email no es válido.");
                _email = email;
            }
        }

        public Categoria Categoria
        {
            get => _categoria;
            set
            {
                if (!Enum.IsDefined(value))
                    throw new ReglaNegocioException("La categoría no es válida.");
                if (value == Categoria.Responsable && Responsable is not null)
                    throw new ReglaNegocioException("Un socio que tiene responsable no puede pasar a ser Responsable.");
                _categoria = value;
            }
        }

        /// <summary>Socio de categoría Responsable que responde por este socio; null si no tiene.</summary>
        public Socio? Responsable { get; private set; }

        public bool EsResponsable => Categoria == Categoria.Responsable;

        /// <summary>Asigna (o quita, con null) el responsable de este socio.</summary>
        public void AsignarResponsable(Socio? responsable)
        {
            if (responsable is not null)
            {
                if (EsResponsable)
                    throw new ReglaNegocioException("Un socio con categoría Responsable no puede tener responsable.");
                if (ReferenceEquals(responsable, this))
                    throw new ReglaNegocioException("Un socio no puede ser su propio responsable.");
                if (!responsable.EsResponsable)
                    throw new ReglaNegocioException("El responsable elegido no tiene la categoría Responsable.");
            }

            Responsable = responsable;
        }

        /// <summary>"Apellido, Nombre (DNI)": para listas desplegables y detalles.</summary>
        public string ObtenerDescripcion() => $"{Apellido}, {Nombre} ({Dni})";
    }
}
