using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Services.Authentification
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<string> RegistrateAsync(string username, string password);
        IEnumerable<Claim> CreateClaims(User user);

        string CreateJwtToken(User user, DateTime expiry, string algorithmType = SecurityAlgorithms.HmacSha256Signature);
    }
}
