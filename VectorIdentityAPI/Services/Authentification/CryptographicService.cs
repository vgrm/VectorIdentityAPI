using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Services.Authentification
{
    public class CryptographicService : ICryptographicService
    {
        private readonly RNGCryptoServiceProvider _cryptoServiceProvider;

        public CryptographicService(RNGCryptoServiceProvider cryptoServiceProvider)
        {
            _cryptoServiceProvider = cryptoServiceProvider;
        }

        public const int DefaultLength = 32;

        public string GenerateSalt(int length = DefaultLength)
        {
            if (length <= 0)
            {
                length = DefaultLength;
            }

            var randomBytes = new byte[length];
            _cryptoServiceProvider.GetBytes(randomBytes);
            var salt = Convert.ToBase64String(randomBytes);

            return salt;
        }

        public string GenerateHash(string password, string saltInBase64)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 3 || string.IsNullOrEmpty(saltInBase64))
            {
                return null;
            }

            try
            {
                var saltBytes = Convert.FromBase64String(saltInBase64);

                var passwordBytes = Encoding.UTF8.GetBytes(password);

                var passwordSalted = passwordBytes.Concat(saltBytes).ToArray();

                var hashBytes = SHA512.Create().ComputeHash(passwordSalted);
                var hash = Convert.ToBase64String(hashBytes);

                return hash;
            }
            catch (FormatException)
            {
                return null;
            }

        }
    }
}
