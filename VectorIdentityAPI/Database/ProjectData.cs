using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class ProjectData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Original { get; set; }
        public double IdentityScore { get; set; }
        public double CorrectnessScore { get; set; }
        public DateTime Date { get; set; }


        public User Owner { get; set; }
        public ProjectSet ProjectSet { get; set; }


        public ICollection<Line> Lines { get; set; }
        public ICollection<Arc> Arcs { get; set; }

        //public ICollection<Match> Matches { get; set; }

    }
}
