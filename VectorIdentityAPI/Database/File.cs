using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Original { get; set; }
        public double Identity { get; set; }
        public double Correctness { get; set; }
        public DateTime Date { get; set; }

        public User Owner { get; set; }
        public Project Project { get; set; }
        public ICollection<Match> Matches { get; set; }
    }
}
