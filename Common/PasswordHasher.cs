using System.Security.Cryptography;

namespace CooperadoraISFT94.Common
{
    /// <summary>Hash de contraseñas con PBKDF2 (incluido en .NET, sin paquetes extra).</summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iteraciones = 100_000;

        public static string Hashear(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iteraciones, HashAlgorithmName.SHA256, HashSize);
            return $"{Iteraciones}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        /// <summary>Un hash guardado con formato inválido se trata como contraseña incorrecta, no como error.</summary>
        public static bool Verificar(string password, string hashGuardado)
        {
            var partes = hashGuardado.Split('.');
            if (partes.Length != 3 || !int.TryParse(partes[0], out var iteraciones) || iteraciones <= 0)
                return false;

            byte[] salt, hashEsperado;
            try
            {
                salt = Convert.FromBase64String(partes[1]);
                hashEsperado = Convert.FromBase64String(partes[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            if (salt.Length == 0 || hashEsperado.Length == 0)
                return false;

            var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(password, salt, iteraciones, HashAlgorithmName.SHA256, hashEsperado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashEsperado);
        }
    }
}
