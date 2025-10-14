using System.Security.Cryptography;
using System.Text;
using Checkin.Api.Common;
using Microsoft.Extensions.Options;

namespace Checkin.Api.Utils
{
    public class PasswordHasher(IOptions<SecuritySettings> securitySettings)
    {
        // Configurações PBKDF2
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        // Pepper não inicializado ainda
        private readonly string Pepper = securitySettings.Value.Pepper;

        public (string Hash, string Salt) HashPassword(string password)
        {
            var saltBytes = new byte[SaltSize];
            RandomNumberGenerator.Fill(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(password + Pepper, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var hashBytes = pbkdf2.GetBytes(KeySize);
            var hash = Convert.ToBase64String(hashBytes);

            return (hash, salt);
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var pbkdf2 = new Rfc2898DeriveBytes(password + Pepper, saltBytes, Iterations, HashAlgorithmName.SHA256);
            var hashBytes = pbkdf2.GetBytes(KeySize);
            var hash = Convert.ToBase64String(hashBytes);

            return hash == storedHash;
        }
    }
}
