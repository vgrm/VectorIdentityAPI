using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class Match
    {
        public int Id { get; set; }

        public Line LineA { get; set; }
        public Line LineB { get; set; }

        public Arc ArcA { get; set; }
        public Arc ArcB { get; set; }


    }
}
