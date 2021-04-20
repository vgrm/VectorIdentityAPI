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
    public class ProjectSetController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectSetController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProjectSet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSet>>> GetProjectSet()
        {
            return await _context.ProjectSet.ToListAsync();
        }

        // GET: api/ProjectSet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectSet>> GetProjectSet(int id)
        {
            var projectSet = await _context.ProjectSet.FindAsync(id);

            if (projectSet == null)
            {
                return NotFound();
            }

            return projectSet;
        }

        // PUT: api/ProjectSet/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectSet(int id, ProjectSet projectSet)
        {
            if (id != projectSet.Id)
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
        public async Task<ActionResult<ProjectSet>> PostProjectSet(ProjectSet projectSet)
        {
            projectSet.OwnerId = 3;
            _context.ProjectSet.Add(projectSet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectSet", new { id = projectSet.Id }, projectSet);
        }

        // DELETE: api/ProjectSet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectSet(int id)
        {
            var projectSet = await _context.ProjectSet.FindAsync(id);
            if (projectSet == null)
            {
                return NotFound();
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
