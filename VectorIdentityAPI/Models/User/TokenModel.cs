using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Models.User
{
    public class TokenModel
    {
        public TokenModel(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}
