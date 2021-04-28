using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vector_control_system_api.Database;

namespace vector_control_system_api.Models.ProjectData
{
    public class ProjectDataResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime DateCreated { get; set; }

        //status: new > processing > analysed
        public string Status { get; set; }
        public bool Original { get; set; }
        public double ScoreIdentity { get; set; }
        public double ScoreCorrectness { get; set; }

        public DateTime DateUploaded { get; set; }
        public DateTime DateUpdated { get; set; }

        public int OwnerId { get; set; }
        public int ProjectSetId { get; set; }

        //public ICollection<Line> Lines { get; set; }
        //public ICollection<Arc> Arcs { get; set; }
    }
}
