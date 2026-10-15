using CooperadoraISFT94.Common;
using CooperadoraISFT94.Common.Exceptions;
using CooperadoraISFT94.Enums;

namespace CooperadoraISFT94.Models
{
    public class Socio : Persona
    {
        public Socio(string nombre, string apellido, string dni, string telefono, string email, Categoria categoria)
            : base(nombre, apellido)
        {
            Dni = NormalizarDni(dni);
            Telefono = NormalizarTelefono(telefono);
            Email = NormalizarEmail(email);
            Categoria = ValidarCategoria(categoria);
            Activo = true;
        }

        public string Dni { get; private set; }

        /// <summary>Opcional: vacío si no se informó.</summary>
        public string Telefono { get; private set; }

        /// <summary>Opcional: vacío si no se informó.</summary>
        public string Email { get; private set; }

        public Categoria Categoria { get; private set; }

        /// <summary>Socio de categoría Responsable que responde por este socio; null si no tiene.</summary>
        public Socio? Responsable { get; private set; }

        /// <summary>False cuando se dio de baja (baja lógica: se conservan sus cuotas y pagos).</summary>
        public bool Activo { get; private set; }

        public bool EsResponsable => Categoria == Categoria.Responsable;

        /// <summary>Asigna (o quita, con null) el responsable de este socio.</summary>
        public void AsignarResponsable(Socio? responsable)
        {
            ValidarResponsable(responsable, Categoria);
            Responsable = responsable;
        }

        /// <summary>
        /// Cambia todos los datos juntos: primero valida todo y recién después modifica, así si algún dato
        /// es inválido el socio queda como estaba.
        /// </summary>
        /// <param name="tieneSociosACargo">
        /// Si hoy responde por otros socios (lo sabe el servicio): en ese caso no puede dejar de ser Responsable,
        /// porque esos socios quedarían a cargo de alguien que ya no lo es.
        /// </param>
        public void ActualizarDatos(string nombre, string apellido, string dni, string telefono, string email,
                                    Categoria categoria, Socio? responsable, bool tieneSociosACargo)
        {
            var nombreValido = Validaciones.ExigirTexto(nombre, "nombre", LargoMaximoNombre);
            var apellidoValido = Validaciones.ExigirTexto(apellido, "apellido", LargoMaximoNombre);
            var dniValido = NormalizarDni(dni);
            var telefonoValido = NormalizarTelefono(telefono);
            var emailValido = NormalizarEmail(email);
            var categoriaValida = ValidarCategoria(categoria);

            if (tieneSociosACargo && categoriaValida != Categoria.Responsable)
                throw new ReglaNegocioException("El socio tiene socios a cargo: reasignalos antes de quitarle la categoría Responsable.");
            ValidarResponsable(responsable, categoriaValida);

            Nombre = nombreValido;
            Apellido = apellidoValido;
            Dni = dniValido;
            Telefono = telefonoValido;
            Email = emailValido;
            Categoria = categoriaValida;
            Responsable = responsable;
        }

        /// <param name="tieneSociosACargo">Un responsable con socios a cargo no se puede dar de baja.</param>
        public void DarDeBaja(bool tieneSociosACargo)
        {
            if (!Activo)
                throw new ReglaNegocioException("El socio ya está dado de baja.");
            if (tieneSociosACargo)
                throw new ReglaNegocioException("El socio tiene socios a cargo: reasignalos antes de darlo de baja.");
            Activo = false;
        }

        public void Reactivar()
        {
            if (Activo)
                throw new ReglaNegocioException("El socio ya está activo.");
            Activo = true;
        }

        /// <summary>Las cuotas nuevas solo se generan para socios activos.</summary>
        public void ExigirActivo()
        {
            if (!Activo)
                throw new ReglaNegocioException("El socio está dado de baja.");
        }

        /// <summary>"Apellido, Nombre (DNI)": para listas desplegables y detalles.</summary>
        public string ObtenerDescripcion() => $"{Apellido}, {Nombre} ({Dni})";

        private void ValidarResponsable(Socio? responsable, Categoria categoria)
        {
            if (responsable is null)
                return;

            if (categoria == Categoria.Responsable)
                throw new ReglaNegocioException("Un socio con categoría Responsable no puede tener responsable.");
            if (ReferenceEquals(responsable, this))
                throw new ReglaNegocioException("Un socio no puede ser su propio responsable.");
            if (!responsable.EsResponsable)
                throw new ReglaNegocioException("El responsable elegido no tiene la categoría Responsable.");
            if (!responsable.Activo)
                throw new ReglaNegocioException("El responsable elegido está dado de baja.");
        }

        private static string NormalizarDni(string? dni)
        {
            var texto = (dni ?? string.Empty).Trim();
            if (!Validaciones.EsDniValido(texto))
                throw new ReglaNegocioException("El DNI debe tener 7 u 8 dígitos, sin puntos.");
            return texto;
        }

        private static string NormalizarTelefono(string? telefono)
        {
            var texto = (telefono ?? string.Empty).Trim();
            if (texto.Length > 0 && !Validaciones.EsTelefonoValido(texto))
                throw new ReglaNegocioException("El teléfono no es válido.");
            return texto;
        }

        private static string NormalizarEmail(string? email)
        {
            var texto = (email ?? string.Empty).Trim();
            if (texto.Length > 0 && !Validaciones.EsEmailValido(texto))
                throw new ReglaNegocioException("El email no es válido.");
            return texto;
        }

        private static Categoria ValidarCategoria(Categoria categoria) =>
            Enum.IsDefined(categoria) ? categoria : throw new ReglaNegocioException("La categoría no es válida.");
    }
}
