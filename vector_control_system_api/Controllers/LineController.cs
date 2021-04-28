using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vector_control_system_api.Database;
using vector_control_system_api.Models.Line;

namespace vector_control_system_api.Controllers
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
        public async Task<ActionResult<IEnumerable<Line>>> GetLines()
        {

            return await _context.Line.ToListAsync();
        }

        // GET: api/Line/LinesMatch
        [HttpGet("LinesMatch")]
        public async Task<ActionResult<IEnumerable<Line>>> GetLinesMatch(int id)
        {
            var lines = _context.Line
                .Where(x => x.Correct && x.ProjectId == id)
                .ToList();
            return Ok(lines);
            //return await _context.Line.ToListAsync();
        }

        // GET: api/Line/LinesIncorrect
        [HttpGet("LinesIncorrect")]
        public async Task<ActionResult<IEnumerable<Line>>> GetLinesIncorrect(int id)
        {
            var lines = _context.Line
                .Where(x => !x.Correct && x.ProjectId == id)
                .ToList();
            return Ok(lines);
            //return await _context.Line.ToListAsync();
        }

        // GET: api/Line/LinesMissing
        [HttpGet("LinesMissing")]
        public async Task<ActionResult<IEnumerable<Line>>> GetLinesMissing(int id)
        {
            var project = _context.ProjectData
                .Where(x => x.Id == id)
                .FirstOrDefault();

            var linesOriginal = _context.Line
                .Where(x => x.ProjectId == project.OriginalProjectId)
                .ToList();

            var linesTest = _context.Line
                .Where(x => x.ProjectId == id)
                .ToList();

            var items = (from x in linesOriginal
                         join y in linesTest
                         on new
                         { x.DX, x.DY, x.DZ, x.Magnitude }
                         equals new
                         { y.DX, y.DY, y.DZ, y.Magnitude }
                         select x)
                .ToList();

            var items2 = linesOriginal
                .Where(x => linesTest
                .All(y => y.DX != x.DX && y.DY != x.DY && y.DZ != x.DZ && y.Magnitude != x.Magnitude))
                .ToList();

            return Ok(items2);
            //return await _context.Line.ToListAsync();
        }

        // GET: api/Line/LinesHandle
        [HttpGet("LinesHandle")]
        public async Task<ActionResult<IEnumerable<Line>>> GetLinesHandle(int id)
        {
            var projectData = _context.ProjectData
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (projectData == null) return Ok();
            List<ProjectData> projects = _context.ProjectData.Where(x => x.ProjectSetId == projectData.ProjectSetId && !x.Original && x.Id != projectData.Id).ToList();
            if (projects == null) return Ok();

            List<Line> lines = _context.Line.Where(x => x.ProjectId == projectData.Id).ToList();
            //List<Arc> arcs = _context.Arc.Where(x => x.ProjectId == projectData.Id).ToList();

            List<Line> matchingLines = new List<Line>();

            foreach (var project in projects)
            {
                List<Line> linesTemp = _context.Line
                    .Include(x => x.Project)
                    .Where(x => x.ProjectId == project.Id)
                    .ToList();

                var items = (from x in lines
                             join y in linesTemp
                             on new
                             { x.Handle, x.DX, x.DY, x.DZ, x.Magnitude, x.Correct }
                             equals new
                             { y.Handle, y.DX, y.DY, y.DZ, y.Magnitude, y.Correct }
                             select y)
                             .ToList();

                if (items != null)
                {
                    matchingLines.AddRange(items);
                }
            }

            var handles = matchingLines.Select(x => new LineResponseModel
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Handle = x.Handle,
                Layer = x.Layer,
                Correct = x.Correct,

                X1 = x.X1,
                Y1 = x.Y1,
                Z1 = x.Z1,
                X2 = x.X2,
                Y2 = x.Y2,
                Z2 = x.Z2,

                Magnitude = x.Magnitude,
                DX = x.DX,
                DY = x.DY,
                DZ = x.DZ,
                Project = x.Project
            });

            return Ok(handles);
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

            if (storedProject == null)
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
