using System.Net.Mail;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Common
{
    /// <summary>Reglas de validación reutilizables por el dominio y los servicios.</summary>
    public static class Validaciones
    {
        public static bool EsDniValido(string dni) =>
            dni.Length is >= 7 and <= 8 && dni.All(char.IsDigit);

        public static bool EsTelefonoValido(string telefono) =>
            telefono.Length is >= 6 and <= 20 &&
            telefono.All(c => char.IsDigit(c) || c is '+' or ' ' or '-' or '(' or ')');

        /// <summary>3 a 30 caracteres: letras, números, punto, guion y guion bajo.</summary>
        public static bool EsNombreUsuarioValido(string nombreUsuario) =>
            nombreUsuario.Length is >= 3 and <= 30 &&
            nombreUsuario.All(c => char.IsAsciiLetterOrDigit(c) || c is '.' or '-' or '_');

        public static bool EsPasswordValida(string password) =>
            password.Length is >= 6 and <= 64;

        public static bool EsAnioValido(int anio) =>
            anio is >= 2000 and <= 2100;

        /// <summary>Los importes y saldos se guardan con centavos, no más.</summary>
        public static bool TieneComoMucho2Decimales(decimal valor) =>
            decimal.Round(valor, 2) == valor;

        public static bool EsEmailValido(string email)
        {
            try
            {
                return new MailAddress(email).Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>Texto obligatorio y acotado. Devuelve el texto ya recortado.</summary>
        public static string ExigirTexto(string? valor, string campo, int largoMaximo)
        {
            var texto = (valor ?? string.Empty).Trim();
            if (texto.Length == 0)
                throw new ReglaNegocioException($"El {campo} es obligatorio.");
            if (texto.Length > largoMaximo)
                throw new ReglaNegocioException($"El {campo} no puede superar los {largoMaximo} caracteres.");
            return texto;
        }

        /// <summary>Monto mayor a cero y con centavos como máximo (cuotas, ingresos, egresos).</summary>
        public static decimal ExigirMontoPositivo(decimal monto, string campo)
        {
            if (monto <= 0)
                throw new ReglaNegocioException($"El {campo} debe ser mayor a cero.");
            return ExigirCentavos(monto, campo);
        }

        /// <summary>Monto sin signo negativo y con centavos como máximo (saldos de caja).</summary>
        public static decimal ExigirMontoNoNegativo(decimal monto, string campo)
        {
            if (monto < 0)
                throw new ReglaNegocioException($"El {campo} no puede ser negativo.");
            return ExigirCentavos(monto, campo);
        }

        private static decimal ExigirCentavos(decimal monto, string campo)
        {
            if (!TieneComoMucho2Decimales(monto))
                throw new ReglaNegocioException($"El {campo} no puede tener más de 2 decimales.");
            return monto;
        }
    }
}
