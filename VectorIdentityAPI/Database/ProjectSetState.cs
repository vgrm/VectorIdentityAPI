using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class ProjectSetState
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public ICollection<ProjectSet> ProjectSets { get; set; }
    }
}
