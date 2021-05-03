using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using vector_control_system_api.Services.Authentification;
using Xunit;

namespace vector_control_system_test
{
    public class CryptographicServiceTest
    {
        private readonly ICryptographicService _cryptoService;

        public CryptographicServiceTest()
        {
            _cryptoService = new CryptographicService(new RNGCryptoServiceProvider());
        }

        [Fact]
        public async Task SaltLength()
        {
            var salt = _cryptoService.GenerateSalt();
            var length = Convert.FromBase64String(salt);
            Assert.Equal(32,length.Length);
        }

        [Fact]
        public async Task SaltRandom()
        {
            var salt1 = _cryptoService.GenerateSalt();
            var salt2 = _cryptoService.GenerateSalt();
            Assert.NotEqual(salt1,salt2);
        }

        [Fact]
        public async Task HashGenerated()
        {
            string pass = "password";
            var salt = _cryptoService.GenerateSalt();
            var hash = _cryptoService.GenerateHash(pass,salt);
            Assert.NotNull(hash);
        }

        [Fact]
        public async Task HashEquals()
        {
            string pass = "password";
            var salt = _cryptoService.GenerateSalt();
            var hash1 = _cryptoService.GenerateHash(pass, salt);
            var hash2 = _cryptoService.GenerateHash(pass, salt);
            Assert.Equal(hash1,hash2);
        }

        [Fact]
        public async Task HashUnique_Salt()
        {
            string pass = "password";
            var salt1 = _cryptoService.GenerateSalt();
            var salt2 = _cryptoService.GenerateSalt();
            var hash1 = _cryptoService.GenerateHash(pass, salt1);
            var hash2 = _cryptoService.GenerateHash(pass, salt2);
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public async Task HashUnique_Pass()
        {
            string pass1 = "password";
            string pass2 = "password2";
            var salt = _cryptoService.GenerateSalt();
            var hash1 = _cryptoService.GenerateHash(pass1, salt);
            var hash2 = _cryptoService.GenerateHash(pass2, salt);
            Assert.NotEqual(hash1, hash2);
        }
    }
}
