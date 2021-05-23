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
    public class LayerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public LayerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Layer
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Layer>>> GetLayer()
        {
            return await _context.Layer.ToListAsync();
        }

        // GET: api/Layer/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Layer>> GetLayer(int id)
        {
            var layer = await _context.Layer.FindAsync(id);

            if (layer == null)
            {
                return NotFound();
            }

            return layer;
        }

        // PUT: api/Layer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutLayer(int id, Layer layer)
        {
            if (id != layer.Id)
            {
                return BadRequest();
            }

            _context.Entry(layer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LayerExists(id))
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

        // POST: api/Layer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Layer>> PostLayer(Layer layer)
        {
            _context.Layer.Add(layer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLayer", new { id = layer.Id }, layer);
        }

        // DELETE: api/Layer/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLayer(int id)
        {
            var layer = await _context.Layer.FindAsync(id);
            if (layer == null)
            {
                return NotFound();
            }

            _context.Layer.Remove(layer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LayerExists(int id)
        {
            return _context.Layer.Any(e => e.Id == id);
        }
    }
}
