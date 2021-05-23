using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vector_control_system_api.Database;

namespace vector_control_system_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserRoleController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/UserRole
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRole()
        {
            return await _context.UserRole.ToListAsync();
        }

        // GET: api/UserRole/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserRole>> GetUserRole(int id)
        {
            var userRole = await _context.UserRole.FindAsync(id);

            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }

        // PUT: api/UserRole/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUserRole(int id, UserRole userRole)
        {
            if (id != userRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(userRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRoleExists(id))
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

        // POST: api/UserRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserRole>> PostUserRole(UserRole userRole)
        {
            _context.UserRole.Add(userRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserRole", new { id = userRole.Id }, userRole);
        }

        // DELETE: api/UserRole/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var userRole = await _context.UserRole.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRole.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserRoleExists(int id)
        {
            return _context.UserRole.Any(e => e.Id == id);
        }
    }
}
