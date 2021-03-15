using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VectorIdentityAPI.Database
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<ProjectSet> ProjectSet { get; set; }
        public DbSet<ProjectData> ProjectData { get; set; }
        public DbSet<Line> Line { get; set; }
        public DbSet<Arc> Arc { get; set; }
        public DbSet<ComparisonData> ComparisonData { get; set; }
        public DbSet<Match> Match { get; set; }






    }

}
