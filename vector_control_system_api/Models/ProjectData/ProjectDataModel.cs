using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Models.ProjectData
{
    public class ProjectDataModel
    {
        //file info
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUploaded { get; set; }
        public int OwnerId { get; set; }
        public int ProjectSetId { get; set; }
        public bool Original { get; set; }
        public string Command { get; set; }
        public string Status { get; set; }
    }
}
