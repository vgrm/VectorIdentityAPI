using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Models.ProjectSet
{
    public class ProjectSetResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public Database.User Owner { get; set; }

        public int StateId { get; set; }
        public Database.ProjectSetState State { get; set; }
    }
}
