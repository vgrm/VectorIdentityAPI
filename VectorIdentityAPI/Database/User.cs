using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class User
    {
        [JsonIgnore] public int Id { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonIgnore] public string PasswordHash { get; set; }
        [JsonIgnore] public string PasswordSalt { get; set; }

        public int RoleId { get; set; }
        [JsonIgnore] public UserRole Role { get; set; }

        [JsonIgnore] public ICollection<ProjectData> Projects { get; set; }
        [JsonIgnore] public ICollection<ProjectSet> ProjectSets { get; set; }
    }
}
