using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Services.Authentification
{
    public class UsernameTakenException : Exception
    {
    }

    public class UsernameOrPasswordInvalidException : Exception
    {
    }

    public class RegistrationException : Exception
    {
    }

    public class ConfigurationMissingException : Exception
    {
    }
}
