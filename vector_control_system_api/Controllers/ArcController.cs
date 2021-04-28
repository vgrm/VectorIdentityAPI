using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vector_control_system_api.Database;
using vector_control_system_api.Models.Arc;

namespace vector_control_system_api.Controllers
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

        // GET: api/Arc/ArcsMatch
        [HttpGet("ArcsMatch")]
        public async Task<ActionResult<IEnumerable<Arc>>> GetArcsMatch(int id)
        {
            var arcs = _context.Arc
                .Where(x => x.Correct && x.ProjectId == id)
                .ToList();
            return Ok(arcs);
        }

        // GET: api/Arc/ArcsIncorrect
        [HttpGet("ArcsIncorrect")]
        public async Task<ActionResult<IEnumerable<Arc>>> GetArcsIncorrect(int id)
        {
            var arcs = _context.Arc
                .Where(x => !x.Correct && x.ProjectId == id)
                .ToList();
            return Ok(arcs);
        }

        // GET: api/Arc/ArcsMissing
        [HttpGet("ArcsMissing")]
        public async Task<ActionResult<IEnumerable<Arc>>> GetArcsMissing(int id)
        {
            var project = _context.ProjectData
                .Where(x => x.Id == id)
                .FirstOrDefault();

            var arcsOriginal = _context.Arc
                .Where(x => x.ProjectId == project.OriginalProjectId)
                .ToList();

            var arcsTest = _context.Arc
                .Where(x => x.ProjectId == id)
                .ToList();

            var items = arcsOriginal
                .Where(x => arcsTest
                .All(y => y.DX != x.DX && y.DY != x.DY && y.DZ != x.DZ && y.Radius != x.Radius && y.AngleEnd != x.AngleEnd && y.AngleStart != x.AngleStart))
                .ToList();

            return Ok(items);
        }

        // GET: api/Arc/ArcHandle
        [HttpGet("ArcsHandle")]
        public async Task<ActionResult<IEnumerable<Arc>>> GetArcsHandle(int id)
        {
            var projectData = _context.ProjectData
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (projectData == null) return Ok();
            List<ProjectData> projects = _context.ProjectData.Where(x => x.ProjectSetId == projectData.ProjectSetId && !x.Original && x.Id != projectData.Id).ToList();
            if (projects == null) return Ok();

            List<Arc> arcs = _context.Arc.Where(x => x.ProjectId == projectData.Id).ToList();

            List<Arc> matchingArcs = new List<Arc>();

            foreach (var project in projects)
            {
                List<Arc> arcsTemp = _context.Arc
                    .Include(x => x.Project)
                    .Where(x => x.ProjectId == project.Id)
                    .ToList();

                var items = (from x in arcs
                             join y in arcsTemp
                             on new
                             { x.Handle, x.DX, x.DY, x.DZ, x.Radius, x.Correct, x.AngleStart, x.AngleEnd }
                             equals new
                             { y.Handle, y.DX, y.DY, y.DZ, y.Radius, y.Correct,y.AngleStart,y.AngleEnd }
                             select y)
                             .ToList();

                if (items != null)
                {
                    matchingArcs.AddRange(items);
                }
            }

            var handles = matchingArcs.Select(x => new ArcResponseModel
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Handle = x.Handle,
                Layer = x.Layer,
                Correct = x.Correct,

                X = x.X,
                Y = x.Y,
                Z = x.Z,

                Radius = x.Radius,
                AngleStart=x.AngleStart,
                AngleEnd=x.AngleEnd,

                DX = x.DX,
                DY = x.DY,
                DZ = x.DZ,

                Project = x.Project
            });

            return Ok(handles);
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
