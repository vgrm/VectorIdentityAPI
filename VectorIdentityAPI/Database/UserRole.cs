using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore] public ICollection<User> Users { get; set; }
    }
}
