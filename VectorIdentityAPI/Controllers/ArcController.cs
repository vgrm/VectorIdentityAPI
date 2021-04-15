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
    public class ArcController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ArcController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Arc
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Arc>>> GetArc()
        {
            return await _context.Arc.ToListAsync();
        }

        // GET: api/Arc/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Arc>> GetArc(int id)
        {
            var arc = await _context.Arc.FindAsync(id);

            if (arc == null)
            {
                return NotFound();
            }

            return arc;
        }

        // PUT: api/Arc/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArc(int id, Arc arc)
        {
            if (id != arc.Id)
            {
                return BadRequest();
            }

            _context.Entry(arc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArcExists(id))
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

        // POST: api/Arc
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Arc>> PostArc(Arc arc)
        {
            _context.Arc.Add(arc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArc", new { id = arc.Id }, arc);
        }

        // DELETE: api/Arc/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArc(int id)
        {
            var arc = await _context.Arc.FindAsync(id);
            if (arc == null)
            {
                return NotFound();
            }

            _context.Arc.Remove(arc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArcExists(int id)
        {
            return _context.Arc.Any(e => e.Id == id);
        }
    }
}
