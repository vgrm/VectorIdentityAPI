using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Database
{
    public class Layer
    {
        //IDs
        public int Id { get; set; }
        public string Name { get; set; }
        public string LineType { get; set; }

        public int ProjectId { get; set; }
        [JsonIgnore] public ProjectData Project { get; set; }
    }
}
