using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class ProjectData
    {
        //file info
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime DateCreated { get; set; }

        public bool Original { get; set; }
        public double IdentityScore { get; set; }
        public double CorrectnessScore { get; set; }

        public DateTime DateUploaded { get; set; }
        public DateTime DateUpdated { get; set; }

        public User Owner { get; set; }
        public ProjectSet ProjectSet { get; set; }


        public ICollection<Line> Lines { get; set; }
        public ICollection<Arc> Arcs { get; set; }

        //public ICollection<Match> Matches { get; set; }

    }
}
