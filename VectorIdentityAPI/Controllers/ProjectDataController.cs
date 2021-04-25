using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;
using VectorIdentityAPI.Models.ProjectData;
using VectorIdentityAPI.Services;


using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace VectorIdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectDataController : ControllerBase
    {
        private readonly DatabaseContext _context;
        //private readonly IAnalyzeService _analyzeService;
        private readonly IBackgroundQueue<ProjectData> _queue;

        public ProjectDataController(DatabaseContext context, IBackgroundQueue<ProjectData> queue)
        {
            _context = context;
            _queue = queue;
        }

        // GET: api/ProjectData
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectData>>> GetProjects(int id)
        {
            if (id != null)
            {
                var projects = _context.ProjectData
                    .Where(project => project.ProjectSetId == id)
                    .ToList();
                return Ok(projects);
            }
            return Ok();
            //return await _context.ProjectData.ToListAsync();
        }

        // GET: api/ProjectData/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectData>> GetProjectData(int id)
        {
            var project = await _context.ProjectData.Include(p => p.Lines).FirstOrDefaultAsync(p => p.Id == id);

            var customProject = new ProjectDataResponseModel
            {
                Id = project.Id,

                Name = project.Name,
                FileType = project.FileType,
                FileData = project.FileData,
                DateCreated = project.DateCreated,

                Status = project.Status,
                Original = project.Original,
                ScoreIdentity = project.ScoreIdentity,
                ScoreCorrectness = project.ScoreCorrectness,

                DateUploaded = project.DateUploaded,
                DateUpdated = project.DateUpdated,

                OwnerId = project.OwnerId,
                ProjectSetId = project.ProjectSetId,

                //Lines = project.Lines,
                //Arcs = null
            };
            //var project = await _context.ProjectData.FirstOrDefaultAsync(p => p.Id == id);

            /*
            var projectData = await _context.ProjectData.FindAsync(id);

            if (projectData == null)
            {
                return NotFound();
            }

            return projectData;
            */

            return Ok(customProject);
        }

        // PUT: api/ProjectData/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectData(int id,ProjectData projectData)
        {
            //check if valid project
            if (id != projectData.Id)
            {
                return BadRequest();
            }

            //update project entries
            if (projectData.Status == "New")
            {
                projectData.Status = "Accepted";
            }


            _context.Entry(projectData).State = EntityState.Modified;

            //save changes
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

            //do more work
            _queue.Enqueue(projectData);

            //return accepted for backgroundwork
            return Accepted();





            //return NoContent();
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
            var projectData = new ProjectData()
            {
                Name = projectDataModel.Name,
                FileType = "dxf",
                DateCreated = DateTime.UtcNow,
                Status = "New",
                Original = false,
                ScoreIdentity = 0,
                ScoreCorrectness = 0,
                DateUploaded = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                OwnerId = 3,
                StateId = -1,
                ProjectSetId = projectDataModel.ProjectSetId

                //FileType = fileExtension
            };

            using (var target = new MemoryStream())
            {
                projectDataModel.File.CopyTo(target);
                projectData.FileData = target.ToArray();
            }

            using (var reader = new StreamReader(projectDataModel.File.OpenReadStream()))
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


        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchProjectData(int id, [FromForm] ProjectDataModel model)
        {

            var projectData = await _context.ProjectData.FirstOrDefaultAsync(x => x.Id == id);

            if (projectData == null)
            {
                return NotFound();
            }
            /*

            var newProjectData = new UpdateCollectionModel
            {
                Name = collection.Name,
                Description = collection.Description,
                ImageId = collection.ImageId
            };

            model.ApplyTo(newProjectData, ModelState);
            */
            //CustomValidation(newCollection);

            //projectData.Name = newProjectData.Name;
            //projectData.Description = newProjectData.Description;
            //projectData.ImageId = newProjectData.ImageId;

            if(model.Command == "ChangeOriginal")
            {

                var projects = _context.ProjectData
                    .Where(project => project.ProjectSetId == projectData.ProjectSetId)
                    .ToList();

                foreach(var project in projects)
                {
                    project.Original = false;
                }
                projectData.Original = true;
            }
            projectData.Status = model.Status;
            //projectData.Original = model.Original;

            await _context.SaveChangesAsync();
            //return CreatedAtAction("GetProjectData", new { id = projectData.Id }, projectData);
            return Ok();
        }
    }
}
