using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vector_control_system_api.Models.ProjectData
{
    public class Offset
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Offset()
        {

        }
        public Offset(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
