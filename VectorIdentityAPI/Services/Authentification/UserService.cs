using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Services.Authentification
{
    public class UserService : IUserService
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IRepository<UserData> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ICryptographicService _cryptographicService;
        private readonly DatabaseContext _databaseContext;

        public UserService(DatabaseContext databaseContext, IConfiguration configuration, ICryptographicService cryptographicService)
        {
            _configuration = configuration;
            _cryptographicService = cryptographicService;
            _databaseContext = databaseContext;
        }

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || username.Length < 2 || password.Length < 2)
            {
                throw new UsernameOrPasswordInvalidException();
            }

            //var user1 = await _userRepository.Get(data => data.Username == username);

            //var comparisonData = await _databaseContext.User.FindAsync();

            var user = _databaseContext.User
                .Include(x => x.Role)
                .Where(x => x.Username == username)
                .FirstOrDefault();

            if (user == null)
            {
                throw new UsernameOrPasswordInvalidException();
            }

            var hash = _cryptographicService.GenerateHash(password, user.PasswordSalt);

            if (hash != user.PasswordHash)
            {
                throw new UsernameOrPasswordInvalidException();
            }

            var token = CreateJwtToken(user, DateTime.Now.AddMinutes(600));

            return token;
        }

        public async Task<string> RegistrateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || username.Length < 2 || password.Length < 2)
            {
                throw new UsernameOrPasswordInvalidException();
            }

            //var user = await _userRepository.Get(data => data.Username == username);

            var user = _databaseContext.User
                .Include(x => x.Role)
                .Where(x => x.Username == username)
                .FirstOrDefault();

            var role = _databaseContext.UserRole
                .Where(x => x.Name == "User")
                .FirstOrDefault();

            //var singleUser = user?.FirstOrDefault();

            if (user != null)
            {
                throw new UsernameTakenException();
            }

            var salt = _cryptographicService.GenerateSalt();
            var hash = _cryptographicService.GenerateHash(password, salt);

            var userData = new User
            {
                Username = username,
                PasswordSalt = salt,
                PasswordHash = hash,
                RoleId = role.Id
            };
            _databaseContext.Add(userData);
            await _databaseContext.SaveChangesAsync();

            //await _userRepository.Create(userData);
            //await _unitOfWork.Save();

            //var createdUsers = await _userRepository.Get(userEntity => userEntity.Username == username);
            //var createdUser = createdUsers?.FirstOrDefault();

            var userCreated = _databaseContext.User
               .Include(x => x.Role)
               .Where(x => x.Username == username)
               .FirstOrDefault();

            if (userCreated == null)
            {
                throw new RegistrationException();
            }

            var token = CreateJwtToken(userCreated, DateTime.Now.AddMinutes(30));

            return token;
        }

        public string CreateJwtToken(User user, DateTime expiry, string algorithmType = SecurityAlgorithms.HmacSha256Signature)
        {
            var userId = user.Id.ToString();
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException();
            }

            if (expiry <= DateTime.Now)
            {
                throw new ArgumentException();
            }

            var secret = _configuration["JWTSecret"];
            if (secret == null)
            {
                throw new ConfigurationMissingException();
            }

            var key = Encoding.ASCII.GetBytes(secret);
            var symmetricSecurityKey = new SymmetricSecurityKey(key);

            var claims = CreateClaims(user);

            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, algorithmType);

            var token = new JwtSecurityToken(
                issuer: "vector-control-system",
                audience: "vector-control-system",
                claims: claims,
                expires: expiry,
                signingCredentials: signingCredentials);

            var tokenSerialized = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenSerialized;
        }
        public IEnumerable<Claim> CreateClaims(User user)
        {
            var claims = new List<Claim>();
            var userId = user.Id.ToString();
            var userRole = user.Role.Name;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException();
            }

            if (!string.IsNullOrWhiteSpace(userRole))
            {
                var roleClaim = new Claim("type", userRole);
                claims.Add(roleClaim);
            }
            var userClaim = new Claim(ClaimTypes.NameIdentifier, userId);

            claims.Add(userClaim);

            return claims;
        }
    }
}
