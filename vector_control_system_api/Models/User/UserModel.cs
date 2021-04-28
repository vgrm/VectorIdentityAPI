using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; }
        public Database.UserRole Role { get; set; }
        public TokenModel Token { get; set; }

    }
}
