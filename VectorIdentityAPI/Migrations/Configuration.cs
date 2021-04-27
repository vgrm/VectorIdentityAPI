using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VectorIdentityAPI.Database;

namespace VectorIdentityAPI.Migrations
{
    public static class Configuration
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectState>().HasData(
                new ProjectState
                {
                    Id = -1,
                    Name = "New",
                },
                new ProjectState
                {
                    Id = -2,
                    Name = "Processing"
                },
                new ProjectState
                {
                    Id = -3,
                    Name = "Analyzing"
                },
                new ProjectState
                {
                    Id = -4,
                    Name = "Analyzed"
                }
            );

            modelBuilder.Entity<ProjectSetState>().HasData(
                new ProjectSetState
                {
                    Id = -1,
                    Name = "Open",
                },
                new ProjectSetState
                {
                    Id = -2,
                    Name = "Closed"
                },
                new ProjectSetState
                {
                    Id = -3,
                    Name = "Private"
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = -1,
                    Name = "Admin",
                },
                new UserRole
                {
                    Id = -2,
                    Name = "User"
                }
            );
        }
    }
}
