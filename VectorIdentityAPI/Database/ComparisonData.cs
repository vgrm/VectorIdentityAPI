using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class ComparisonData
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProjectData ProjectA { get; set; }
        public ProjectData ProjectB { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}
