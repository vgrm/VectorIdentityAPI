using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class Match
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Type { get; set; }

        //matching lines
        public int LineOriginalId { get; set; }
        [JsonIgnore] public Line LineOriginal { get; set; }
        public int LineTestId { get; set; }
        [JsonIgnore] public Line LineTest { get; set; }

        //matching arcs
        public int ArcOriginalId { get; set; }
        [JsonIgnore] public Arc ArcOriginal { get; set; }
        public int ArcTestId { get; set; }
        [JsonIgnore] public Arc ArcTest { get; set; }

        //projects
        //public int OriginalProjectId { get; set; }
        //[JsonIgnore] public ProjectData OriginalProject { get; set; }


        //public int TestProjectId { get; set; }
        //[JsonIgnore] public ProjectData TestProject { get; set; }

    }
}
