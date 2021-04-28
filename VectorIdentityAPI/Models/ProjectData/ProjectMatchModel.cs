using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Models.ProjectData
{
    public class ProjectMatchModel
    {
        public int Id { get; set; }



        public string Name { get; set; }
        public string Info { get; set; }
        public string Type { get; set; }

        //matching lines
        public int LineOriginalId { get; set; }
        public int LineTestId { get; set; }

        //matching arcs
        public int ArcOriginalId { get; set; }
        public int ArcTestId { get; set; }

        //projects
        //public int OriginalProjectId { get; set; }
        //[JsonIgnore] public ProjectData OriginalProject { get; set; }


        //public int TestProjectId { get; set; }
        //[JsonIgnore] public ProjectData TestProject { get; set; }
    }
}
