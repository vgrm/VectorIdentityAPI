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
    public class ProjectSetStateController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectSetStateController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProjectSetState
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSetState>>> GetProjectSetState()
        {
            return await _context.ProjectSetState.ToListAsync();
        }

        // GET: api/ProjectSetState/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectSetState>> GetProjectSetState(int id)
        {
            var projectSetState = await _context.ProjectSetState.FindAsync(id);

            if (projectSetState == null)
            {
                return NotFound();
            }

            return projectSetState;
        }

        // PUT: api/ProjectSetState/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectSetState(int id, ProjectSetState projectSetState)
        {
            if (id != projectSetState.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectSetState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectSetStateExists(id))
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

        // POST: api/ProjectSetState
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectSetState>> PostProjectSetState(ProjectSetState projectSetState)
        {
            _context.ProjectSetState.Add(projectSetState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectSetState", new { id = projectSetState.Id }, projectSetState);
        }

        // DELETE: api/ProjectSetState/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectSetState(int id)
        {
            var projectSetState = await _context.ProjectSetState.FindAsync(id);
            if (projectSetState == null)
            {
                return NotFound();
            }

            _context.ProjectSetState.Remove(projectSetState);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectSetStateExists(int id)
        {
            return _context.ProjectSetState.Any(e => e.Id == id);
        }
    }
}
