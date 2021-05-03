using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Database
{
    public class Arc
    {
        //IDs
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Layer { get; set; }
        public bool Correct { get; set; }

        //center coordinates
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        //data
        public double Radius { get; set; }
        public double AngleStart { get; set; }
        public double AngleEnd { get; set; }

        //plane direction
        public double DX { get; set; }
        public double DY { get; set; }
        public double DZ { get; set; }

        public int ProjectId { get; set; }
        [JsonIgnore] public ProjectData Project { get; set; }

        //[JsonIgnore] public ICollection<Match> OriginalMatches { get; set; }
        //[JsonIgnore] public ICollection<Match> TestMatches { get; set; }
    }
}
