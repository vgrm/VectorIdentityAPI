using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class ProjectSet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        [JsonIgnore] public User? Owner { get; set; }

        [JsonIgnore] public ICollection<ProjectData> Projects { get; set; }
    }
}
