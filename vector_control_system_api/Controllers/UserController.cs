using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using vector_control_system_api.Database;
using vector_control_system_api.Models.Error;
using vector_control_system_api.Models.User;
using vector_control_system_api.Services.Authentification;

namespace vector_control_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IUserService _userService;

        public UserController(DatabaseContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);

            var user = await _context.User
                .Include(x=>x.Role)
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return BadRequest();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{username}")]
        [Authorize]
        public async Task<IActionResult> PutUser(string username, User userData)
        {
            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            var adminClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "type");
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);


            var user = await _context.User
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (userId != user.Id && adminClaim.Value != "Admin")
            {
                return BadRequest();
            }

            if (adminClaim.Value == "Admin")
            {
                user.RoleId = userData.RoleId;
            }
            user.FirstName = userData.FirstName;
            user.LastName = userData.LastName;
            user.Email = userData.Email;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("signup")]
        public async Task<ActionResult<UserModel>> Signup([FromBody] SigninModel signinModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var token = await _userService.RegistrateAsync(signinModel.Username, signinModel.Password);
                var tokenModel = new TokenModel(token);

                var user = _context.User
                    .Include(x => x.Role)
                    .Where(x => x.Username == signinModel.Username)
                    .FirstOrDefault();

                var userModel = new UserModel
                {
                    Id = user.Id,

                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,

                    RoleId = user.RoleId,
                    Role = user.Role,
                    Token = tokenModel
                };

                return Ok(userModel);
            }
            catch (UsernameTakenException)
            {
                return BadRequest(new ErrorResponseModel("Username is taken"));
            }
            catch (RegistrationException)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("signin")]

        public async Task<ActionResult<UserModel>> Signin([FromBody] SigninModel signinModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var token = await _userService.AuthenticateAsync(signinModel.Username, signinModel.Password);
                var tokenModel = new TokenModel(token);


                var user = _context.User
                    .Include(x => x.Role)
                    .Where(x => x.Username == signinModel.Username)
                    .FirstOrDefault();

                var userModel = new UserModel
                {
                    Id = user.Id,

                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,

                    RoleId = user.RoleId,
                    Role = user.Role,
                    Token = tokenModel
                };

                return Ok(userModel);
            }
            catch (UsernameOrPasswordInvalidException)
            {
                return BadRequest(new ErrorResponseModel("Username or password is invalid"));
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{username}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string username)
        {

            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            var adminClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "type");
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);


            var user = await _context.User
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (userId != user.Id && adminClaim.Value != "Admin")
            {
                return BadRequest();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
