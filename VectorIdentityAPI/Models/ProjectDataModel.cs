using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Models
{
    public class ProjectDataModel
    {
        //file info
        public string Name { get; set; }
        public IFormFile file { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
