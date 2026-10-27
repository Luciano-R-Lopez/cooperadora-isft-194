using System.Net.Mail;
using CooperadoraISFT94.Common.Exceptions;

namespace CooperadoraISFT94.Common
{
    // Validaciones que comparten varias entidades. Cada una lanza NegocioException con un mensaje para el usuario.
    public static class Validaciones
    {
        // Para los datos obligatorios que son otra entidad (usuario, socio, medio de pago...).
        public static T ExigirDato<T>(T? valor, string mensaje) where T : class
        {
            if (valor == null)
            {
                throw new NegocioException(mensaje);
            }
            return valor;
        }

        // campo: "categoría", "acción"... para el mensaje "La categoría no es válida."
        public static T ExigirEnum<T>(T valor, string campo) where T : struct, Enum
        {
            if (!Enum.IsDefined(valor))
            {
                throw new NegocioException("La " + campo + " no es válida.");
            }
            return valor;
        }

        public static string ExigirTexto(string? valor, string campo, int largoMaximo)
        {
            string texto = (valor ?? string.Empty).Trim();
            if (texto.Length == 0)
            {
                throw new NegocioException("El " + campo + " no puede estar vacío.");
            }
            if (texto.Length > largoMaximo)
            {
                throw new NegocioException("El " + campo + " no puede tener más de " + largoMaximo + " caracteres.");
            }
            return texto;
        }

        // Monto mayor a cero y con centavos como máximo (cuotas, ingresos, egresos).
        public static decimal ExigirMontoPositivo(decimal monto, string campo)
        {
            if (monto <= 0)
            {
                throw new NegocioException("El " + campo + " debe ser mayor a cero.");
            }
            return ExigirCentavos(monto, campo);
        }

        // Monto sin signo negativo y con centavos como máximo (saldos de caja).
        public static decimal ExigirMontoNoNegativo(decimal monto, string campo)
        {
            if (monto < 0)
            {
                throw new NegocioException("El " + campo + " no puede ser negativo.");
            }
            return ExigirCentavos(monto, campo);
        }

        public static int ExigirAnio(int anio)
        {
            if (anio < Configuracion.AnioMinimo || anio > Configuracion.AnioMaximo)
            {
                throw new NegocioException("El año debe estar entre " + Configuracion.AnioMinimo + " y " + Configuracion.AnioMaximo + ".");
            }
            return anio;
        }

        public static bool EsDniValido(string dni)
        {
            return dni.Length >= 7 && dni.Length <= 8 && dni.All(char.IsDigit);
        }

        public static bool EsTelefonoValido(string telefono)
        {
            return telefono.Length >= 6 && telefono.Length <= 20
                && telefono.All(c => char.IsDigit(c) || c == '+' || c == ' ' || c == '-' || c == '(' || c == ')');
        }

        // De 3 a 30 caracteres: letras, números, punto, guion y guion bajo.
        public static bool EsNombreUsuarioValido(string nombreUsuario)
        {
            return nombreUsuario.Length >= 3 && nombreUsuario.Length <= 30
                && nombreUsuario.All(c => char.IsAsciiLetterOrDigit(c) || c == '.' || c == '-' || c == '_');
        }

        public static bool EsPasswordValida(string password)
        {
            return password.Length >= 6 && password.Length <= 64;
        }

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

        private static decimal ExigirCentavos(decimal monto, string campo)
        {
            if (decimal.Round(monto, 2) != monto)
            {
                throw new NegocioException("El " + campo + " no puede tener más de 2 decimales.");
            }
            return monto;
        }
    }
}
