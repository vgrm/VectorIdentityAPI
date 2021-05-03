﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Models.Error
{
    public class ErrorResponseModel
    {
        public ErrorResponseModel(string error)
        {
            Error = error;
        }

        public string Error { get; }
    }
}
