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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user_data");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.HasIndex(e => e.Email).HasDatabaseName("user_data_email_key").IsUnique();
                entity.HasIndex(e => e.Username).HasDatabaseName("user_data_username_key").IsUnique();

                entity.Property(e => e.Username).HasColumnName("username").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();

                entity.Property(e => e.FirstName).HasColumnName("firstname");
                entity.Property(e => e.LastName).HasColumnName("lastname");

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();
            });

            modelBuilder.Entity<ProjectSet>(entity =>
            {
                entity.ToTable("projectset");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");
                entity.HasOne(e => e.Owner)
                    .WithMany(e => e.ProjectSets)
                    .HasForeignKey(e => e.OwnerId)
                    .HasConstraintName("projectset_owner_id_fkey")
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);

            });

            modelBuilder.Entity<ProjectData>(entity =>
            {
                entity.ToTable("projectdata");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.FileType).HasColumnName("file_type");
                entity.Property(e => e.FileData).HasColumnName("file_data");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
                entity.Property(e => e.Original).HasColumnName("original").IsRequired();

                entity.Property(e => e.ScoreIdentity).HasColumnName("score_identity");
                entity.Property(e => e.ScoreCorrectness).HasColumnName("score_correctness");

                entity.Property(e => e.DateUploaded)
                    .HasColumnName("date_uploaded")
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("date")
                    .IsRequired();

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");
                entity.HasOne(e => e.Owner)
                    .WithMany(e => e.Projects)
                    .HasForeignKey(e => e.OwnerId)
                    .HasConstraintName("projectdata_owner_id_fkey")
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(e => e.ProjectSetId).HasColumnName("projectset_id");
                entity.HasOne(e => e.ProjectSet)
                    .WithMany(e => e.Projects)
                    .HasForeignKey(d => d.ProjectSetId)
                    .HasConstraintName("projectdata_projectset_id_fkey");

            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.ToTable("line");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Handle).HasColumnName("handle");
                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.X1).HasColumnName("x1");
                entity.Property(e => e.Y1).HasColumnName("y1");
                entity.Property(e => e.Z1).HasColumnName("z1");

                entity.Property(e => e.X2).HasColumnName("x2");
                entity.Property(e => e.Y2).HasColumnName("y2");
                entity.Property(e => e.Z2).HasColumnName("z2");

                entity.Property(e => e.DX).HasColumnName("dx");
                entity.Property(e => e.DY).HasColumnName("dy");
                entity.Property(e => e.DZ).HasColumnName("dz");

                entity.Property(e => e.Magnitude).HasColumnName("magnitude");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");
                entity.HasOne(e => e.Project)
                    .WithMany(e => e.Lines)
                    .HasForeignKey(e => e.ProjectId)
                    .HasConstraintName("line_projectdata_id_fkey")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Arc>(entity =>
            {
                entity.ToTable("arc");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Handle).HasColumnName("handle");
                entity.Property(e => e.Layer).HasColumnName("layer");

                entity.Property(e => e.X).HasColumnName("x");
                entity.Property(e => e.Y).HasColumnName("y");
                entity.Property(e => e.Z).HasColumnName("z");

                entity.Property(e => e.Radius).HasColumnName("radius");
                entity.Property(e => e.AngleStart).HasColumnName("angle_start");
                entity.Property(e => e.AngleEnd).HasColumnName("angle_end");

                entity.Property(e => e.DX).HasColumnName("dx");
                entity.Property(e => e.DY).HasColumnName("dy");
                entity.Property(e => e.DZ).HasColumnName("dz");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");
                entity.HasOne(e => e.Project)
                    .WithMany(e => e.Arcs)
                    .HasForeignKey(e => e.ProjectId)
                    .HasConstraintName("arc_projectdata_id_fkey")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("match");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Info).HasColumnName("info");
                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.LineOriginalId).HasColumnName("line_original_id");
                entity.HasOne(e => e.LineOriginal)
                      .WithMany(e => e.OriginalMatches)
                      .HasForeignKey(e => e.LineOriginalId)
                      .HasConstraintName("match_line_original_id")
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.LineTestId).HasColumnName("line_test_id");
                entity.HasOne(e => e.LineTest)
                      .WithMany(e => e.TestMatches)
                      .HasForeignKey(e => e.LineTestId)
                      .HasConstraintName("match_line_test_id")
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.ArcOriginalId).HasColumnName("arc_original_id");
                entity.HasOne(e => e.ArcOriginal)
                      .WithMany(e => e.OriginalMatches)
                      .HasForeignKey(e => e.ArcOriginalId)
                      .HasConstraintName("match_arc_original_id")
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.ArcTestId).HasColumnName("arc_test_id");
                entity.HasOne(e => e.ArcTest)
                      .WithMany(e => e.TestMatches)
                      .HasForeignKey(e => e.ArcTestId)
                      .HasConstraintName("match_arc_test_id")
                      .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.OriginalProjectId).HasColumnName("original_project_id");
                entity.HasOne(e => e.OriginalProject)
                    .WithMany(e => e.OriginalMatches)
                    .HasForeignKey(e => e.OriginalProjectId)
                    .HasConstraintName("match_originalprojectdata_id_fkey")
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.TestProjectId).HasColumnName("test_project_id");
                entity.HasOne(e => e.TestProject)
                      .WithMany(e => e.TestMatches)
                      .HasForeignKey(e => e.TestProjectId)
                      .HasConstraintName("match_testprojectdata_id_fkey")
                      .OnDelete(DeleteBehavior.NoAction);
            });

        }




    }

}
