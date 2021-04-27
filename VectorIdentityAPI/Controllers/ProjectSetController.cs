using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;
using VectorIdentityAPI.Models.ProjectSet;

namespace VectorIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSetController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectSetController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProjectSet
        [HttpGet("owned")]
        [Authorize]
        //public async Task<ActionResult<IEnumerable<ProjectSetRes>>> GetProjectSetsOwned()
        public IActionResult GetProjectSetsOwned()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();
            }

            int id = int.Parse(userId.Value);
            var user = _context.User.Find(id);
            //var user =  _context.User.FindAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            var projectSets = _context.ProjectSet
                .Include(x => x.Owner)
                .Include(x => x.State)
                .Where(x => x.OwnerId == user.Id)
                .ToList()
                .Select(x => new ProjectSetResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status,
                    OwnerId = x.OwnerId,
                    Owner = x.Owner,
                    StateId = x.StateId,
                    State = x.State
                });

            return Ok(projectSets);
        }


        // GET: api/ProjectSet
        [HttpGet("other")]
        [Authorize]
        //public async Task<ActionResult<IEnumerable<ProjectSet>>> GetProjectSetsOther()
        public IActionResult GetProjectSetsOther()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();
            }

            int id = int.Parse(userId.Value);
            var user = _context.User.Find(id);
            //var user =  _context.User.FindAsync(id);

            if (user == null)
            {
                return BadRequest();
            }


            var projectSets = _context.ProjectSet
                .Include(x => x.Owner)
                .Include(x => x.State)
                .Where(x => x.OwnerId != user.Id && x.StateId != -3)
                .ToList()
                .Select(x => new ProjectSetResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status,
                    OwnerId = x.OwnerId,
                    Owner = x.Owner,
                    StateId = x.StateId,
                    State = x.State
                });

            return Ok(projectSets);
        }

        // GET: api/ProjectSet
        [HttpGet]
        [Authorize(Policy = "Admin")]
        //public async Task<ActionResult<IEnumerable<ProjectSet>>> GetProjectSet()
        public IActionResult GetProjectSet()
        {

            var projectSets = _context.ProjectSet
                .Include(x => x.Owner)
                .Include(x => x.State)
                .ToList()
                .Select(x => new ProjectSetResponseModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Status = x.Status,
                    OwnerId = x.OwnerId,
                    Owner = x.Owner,
                    StateId = x.StateId,
                    State = x.State
                });

            return Ok(projectSets);
        }

        // GET: api/ProjectSet/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProjectSet>> GetProjectSet(int id)
        {
            /*
            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);
            var user = _context.User.Find(userId);

            //var user =  _context.User.FindAsync(id);

            if (user == null)
            {
                return BadRequest();
            }
            */
            var projectSet = await _context.ProjectSet
                .Include(x => x.Owner)
                .Include(x => x.State)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (projectSet == null)
            {
                return BadRequest();
            }

            var projectSetResponse = new ProjectSetResponseModel
            {
                Id = projectSet.Id,
                Name = projectSet.Name,
                Description = projectSet.Description,
                Status = projectSet.Status,
                OwnerId = projectSet.OwnerId,
                Owner = projectSet.Owner,
                StateId = projectSet.StateId,
                State = projectSet.State
            };

            return Ok(projectSetResponse);
        }

        // PUT: api/ProjectSet/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProjectSet(int id, ProjectSet projectSet)
        {
            if (id != projectSet.Id)
            {
                return BadRequest();
            }

            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);
            var user = _context.User.Find(userId);

            int stateId = -3;
            int.TryParse(projectSet.Status, out stateId);
            projectSet.StateId = stateId;
            //var user =  _context.User.FindAsync(id);

            if (user == null || user.Id != projectSet.OwnerId)
            {
                return BadRequest();
            }

            _context.Entry(projectSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectSetExists(id))
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

        // POST: api/ProjectSet
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProjectSet>> PostProjectSet(ProjectSet projectSet)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest();
            }

            int id = int.Parse(userId.Value);
            var user = await _context.User.FindAsync(id);

            int stateId = -3;
            int.TryParse(projectSet.Status, out stateId);

            if (user == null)
            {
                return BadRequest();
            }

            projectSet.OwnerId = user.Id;
            projectSet.StateId = stateId;
            _context.ProjectSet.Add(projectSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectSet", new { id = projectSet.Id }, projectSet);
        }

        // DELETE: api/ProjectSet/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProjectSet(int id)
        {
            var projectSet = await _context.ProjectSet.FindAsync(id);
            if (projectSet == null)
            {
                return NotFound();
            }

            if (id != projectSet.Id)
            {
                return BadRequest();
            }

            var userClaim = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return BadRequest();
            }

            int userId = int.Parse(userClaim.Value);
            var user = _context.User.Find(userId);

            //var user =  _context.User.FindAsync(id);

            if (user == null || user.Id != projectSet.OwnerId)
            {
                return BadRequest();
            }

            _context.ProjectSet.Remove(projectSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectSetExists(int id)
        {
            return _context.ProjectSet.Any(e => e.Id == id);
        }
    }
}
