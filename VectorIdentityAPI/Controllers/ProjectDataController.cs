using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;
using VectorIdentityAPI.Models;

namespace VectorIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDataController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProjectDataController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ProjectData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectData>>> GetProjectData()
        {
            return await _context.ProjectData.ToListAsync();
        }

        // GET: api/ProjectData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectData>> GetProjectData(int id)
        {
            var projectData = await _context.ProjectData.FindAsync(id);

            if (projectData == null)
            {
                return NotFound();
            }

            return projectData;
        }

        // PUT: api/ProjectData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectData(int id, ProjectData projectData)
        {
            if (id != projectData.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectDataExists(id))
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

        /*
        // POST: api/ProjectData
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjectData>> PostProjectData(ProjectData projectData)
        {
            _context.ProjectData.Add(projectData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectData", new { id = projectData.Id }, projectData);
        }
        */
        [HttpPost]
        public async Task<ActionResult<ProjectData>> PostProjectData([FromForm] ProjectDataModel projectDataModel)
        {
            /*
            if (files != null)
            {
                
                if (files.Length > 0)
                {
                    //Getting FileName
                    var fileName = Path.GetFileName(files.FileName);
                    //Getting file Extension
                    var fileExtension = Path.GetExtension(fileName);
                    // concatenating  FileName + FileExtension
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var projectData = new ProjectData()
                    {
                        Name = newFileName,
                        FileType = fileExtension
                    };

                    using (var target = new MemoryStream())
                    {
                        files.CopyTo(target);
                        projectData.FileData = target.ToArray();
                    }

                    _context.ProjectData.Add(projectData);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetProjectData", new { id = projectData.Id }, projectData);
                }
                
            }
        */
            var projectData = new ProjectData()
            {
                Name = projectDataModel.Name,
                //FileType = fileExtension
            };

            using (var target = new MemoryStream())
            {
                projectDataModel.file.CopyTo(target);
                projectData.FileData = target.ToArray();
            }

            using (var reader = new StreamReader(projectDataModel.file.OpenReadStream()))
            {
                var a = await reader.ReadToEndAsync();
            }

            _context.ProjectData.Add(projectData);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProjectData", new { id = projectData.Id }, projectData);
        }
       
        // DELETE: api/ProjectData/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectData(int id)
        {
            var projectData = await _context.ProjectData.FindAsync(id);
            if (projectData == null)
            {
                return NotFound();
            }

            _context.ProjectData.Remove(projectData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectDataExists(int id)
        {
            return _context.ProjectData.Any(e => e.Id == id);
        }
    }
}
