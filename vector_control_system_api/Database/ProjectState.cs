using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Database
{
    public class ProjectState
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public ICollection<ProjectData> Projects { get; set; }
    }
}
