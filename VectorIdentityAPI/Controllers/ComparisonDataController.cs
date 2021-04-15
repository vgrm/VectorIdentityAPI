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
    public class ComparisonDataController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ComparisonDataController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ComparisonData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComparisonData>>> GetComparisonData()
        {
            return await _context.ComparisonData.ToListAsync();
        }

        // GET: api/ComparisonData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComparisonData>> GetComparisonData(int id)
        {
            var comparisonData = await _context.ComparisonData.FindAsync(id);

            if (comparisonData == null)
            {
                return NotFound();
            }

            return comparisonData;
        }

        // PUT: api/ComparisonData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComparisonData(int id, ComparisonData comparisonData)
        {
            if (id != comparisonData.Id)
            {
                return BadRequest();
            }

            _context.Entry(comparisonData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComparisonDataExists(id))
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

        // POST: api/ComparisonData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ComparisonData>> PostComparisonData(ComparisonData comparisonData)
        {
            _context.ComparisonData.Add(comparisonData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComparisonData", new { id = comparisonData.Id }, comparisonData);
        }

        // DELETE: api/ComparisonData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComparisonData(int id)
        {
            var comparisonData = await _context.ComparisonData.FindAsync(id);
            if (comparisonData == null)
            {
                return NotFound();
            }

            _context.ComparisonData.Remove(comparisonData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComparisonDataExists(int id)
        {
            return _context.ComparisonData.Any(e => e.Id == id);
        }
    }
}
