using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class Line
    {
        //IDs
        public int Id { get; set; }
        public string Handle { get; set; }
        public string Layer { get; set; }
        public bool Correct { get; set; }
        //Data
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double Z1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public double Z2 { get; set; }

        //lenght
        public double Magnitude { get; set; }

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
