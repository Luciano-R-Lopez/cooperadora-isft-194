using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Enums;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Domain
{
    public class Socio : Persona
    {
        private string dni = string.Empty;
        private string telefono = string.Empty;
        private string email = string.Empty;
        private Categoria categoria;

        public Socio(int id, string nombre, string apellido, string dni, string telefono, string email, Categoria categoria)
            : base(id, nombre, apellido)
        {
            Dni = dni;
            Telefono = telefono;
            Email = email;
            Categoria = categoria;
            Activo = true;
        }

        public string Dni
        {
            get { return dni; }
            set
            {
                string texto = (value ?? string.Empty).Trim();
                if (!Validaciones.EsDniValido(texto))
                {
                    throw new NegocioException("El DNI debe tener 7 u 8 números, sin puntos.");
                }
                dni = texto;
            }
        }

        // Opcional: vacío si no se informó.
        public string Telefono
        {
            get { return telefono; }
            set
            {
                string texto = (value ?? string.Empty).Trim();
                if (texto.Length > 0 && !Validaciones.EsTelefonoValido(texto))
                {
                    throw new NegocioException("El teléfono no es válido (de 6 a 20 caracteres: números, espacios, +, - y paréntesis).");
                }
                telefono = texto;
            }
        }

        // Opcional: vacío si no se informó.
        public string Email
        {
            get { return email; }
            set
            {
                string texto = (value ?? string.Empty).Trim();
                if (texto.Length > 0 && !Validaciones.EsEmailValido(texto))
                {
                    throw new NegocioException("El email ingresado no es válido (ejemplo: nombre@dominio.com).");
                }
                email = texto;
            }
        }

        public Categoria Categoria
        {
            get { return categoria; }
            set { categoria = Validaciones.ExigirEnum(value, "categoría"); }
        }

        // Socio de categoría Responsable que paga por este socio; null si no tiene.
        public Socio? Responsable { get; private set; }

        // False cuando se dio de baja (baja lógica: se conservan sus cuotas y pagos).
        public bool Activo { get; private set; }

        public bool EsResponsable
        {
            get { return Categoria == Categoria.Responsable; }
        }

        public void AsignarResponsable(Socio? responsable)
        {
            if (responsable != null)
            {
                if (EsResponsable)
                {
                    throw new NegocioException("Un socio con categoría Responsable no puede tener responsable.");
                }
                if (responsable.Id == Id)
                {
                    throw new NegocioException("Un socio no puede ser su propio responsable.");
                }
                if (!responsable.EsResponsable)
                {
                    throw new NegocioException("El responsable elegido no tiene la categoría Responsable.");
                }
                if (!responsable.Activo)
                {
                    throw new NegocioException("El responsable elegido está dado de baja.");
                }
            }
            Responsable = responsable;
        }

        public void DarDeBaja()
        {
            if (!Activo)
            {
                throw new NegocioException("El socio ya está dado de baja.");
            }
            Activo = false;
        }

        public void Reactivar()
        {
            if (Activo)
            {
                throw new NegocioException("El socio ya está activo.");
            }
            Activo = true;
        }

        // Las cuotas nuevas solo se generan para socios activos.
        public void ExigirActivo()
        {
            if (!Activo)
            {
                throw new NegocioException("El socio " + ObtenerDescripcion() + " está dado de baja.");
            }
        }

        public string ObtenerDescripcion()
        {
            return Apellido + ", " + Nombre + " (" + Dni + ")";
        }
    }
}
