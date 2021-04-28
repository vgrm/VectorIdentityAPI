using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectStateController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectStateController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProjectState
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectState>>> GetProjectState()
        {
            return await _context.ProjectState.ToListAsync();
        }

        // GET: api/ProjectState/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectState>> GetProjectState(int id)
        {
            var projectState = await _context.ProjectState.FindAsync(id);

            if (projectState == null)
            {
                return NotFound();
            }

            return projectState;
        }

        // PUT: api/ProjectState/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectState(int id, ProjectState projectState)
        {
            if (id != projectState.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectStateExists(id))
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

        // POST: api/ProjectState
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectState>> PostProjectState(ProjectState projectState)
        {
            _context.ProjectState.Add(projectState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectState", new { id = projectState.Id }, projectState);
        }

        // DELETE: api/ProjectState/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectState(int id)
        {
            var projectState = await _context.ProjectState.FindAsync(id);
            if (projectState == null)
            {
                return NotFound();
            }

            _context.ProjectState.Remove(projectState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectStateExists(int id)
        {
            return _context.ProjectState.Any(e => e.Id == id);
        }
    }
}
