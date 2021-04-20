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
    public class LineController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LineController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Line
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLine(string projectId)
        {
            projectId = "11";
            var lines = _context.Line.Where(line => line.ProjectId.ToString() == projectId);

            return Ok(lines);
            //return await _context.Line.ToListAsync();
        }

        // GET: api/Line/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Line>> GetLine(int id)
        {
            var line = await _context.Line.FindAsync(id);

            if (line == null)
            {
                return NotFound();
            }

            return line;
        }

        // PUT: api/Line/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLine(int id, Line line)
        {
            if (id != line.Id)
            {
                return BadRequest();
            }

            _context.Entry(line).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineExists(id))
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

        // POST: api/Line
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            //check for nulls

            var storedProject = await _context.ProjectData.FirstOrDefaultAsync(p => p.Id == line.ProjectId);

            if(storedProject == null)
            {
                //return BadRequest();
            }

            _context.Line.Add(line);
            await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction("GetLine", new { id = line.Id }, line);
        }

        // DELETE: api/Line/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLine(int id)
        {
            var line = await _context.Line.FindAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            _context.Line.Remove(line);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LineExists(int id)
        {
            return _context.Line.Any(e => e.Id == id);
        }
    }
}
