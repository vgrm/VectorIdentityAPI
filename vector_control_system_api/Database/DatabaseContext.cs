using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vector_control_system_api.Migrations;

namespace vector_control_system_api.Database
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
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<ProjectSet> ProjectSet { get; set; }
        public DbSet<ProjectSetState> ProjectSetState { get; set; }

        public DbSet<ProjectData> ProjectData { get; set; }
        public DbSet<ProjectState> ProjectState { get; set; }

        public DbSet<Line> Line { get; set; }
        public DbSet<Arc> Arc { get; set; }

        public DbSet<Layer> Layer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user_data");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Username).HasDatabaseName("user_data_username_key").IsUnique();

                entity.Property(e => e.Username).HasColumnName("username").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email");

                entity.Property(e => e.FirstName).HasColumnName("firstname");
                entity.Property(e => e.LastName).HasColumnName("lastname");

                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.PasswordSalt).HasColumnName("password_salt").IsRequired();

                entity.Property(e => e.RoleId).HasColumnName("role_id");
                entity.HasOne(e => e.Role)
                    .WithMany(e => e.Users)
                    .HasForeignKey(e => e.RoleId)
                    .HasConstraintName("user_data_role_id_fkey")
                    .OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<ProjectState>(entity =>
            {
                entity.ToTable("projectdata_state");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<ProjectSetState>(entity =>
            {
                entity.ToTable("projectset_state");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<ProjectSet>(entity =>
            {
                entity.ToTable("projectset");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();

                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.HasOne(e => e.State)
                    .WithMany(e => e.ProjectSets)
                    .HasForeignKey(e => e.StateId)
                    .HasConstraintName("projectset_state_id_fkey")
                    .OnDelete(DeleteBehavior.NoAction);

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
                entity.Property(e => e.OriginalProjectId).HasColumnName("original_project_id");

                entity.Property(e => e.FileType).HasColumnName("file_type");
                entity.Property(e => e.FileData).HasColumnName("file_data");

                entity.Property(e => e.DateCreated)
                    .HasColumnName("date_created")
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
                entity.Property(e => e.Original).HasColumnName("original").IsRequired();

                entity.Property(e => e.ScoreIdentity).HasColumnName("score_identity");
                entity.Property(e => e.ScoreCorrectness).HasColumnName("score_correctness");

                entity.Property(e => e.DateUploaded)
                    .HasColumnName("date_uploaded")
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.StateId).HasColumnName("state_id");
                entity.HasOne(e => e.State)
                    .WithMany(e => e.Projects)
                    .HasForeignKey(e => e.StateId)
                    .HasConstraintName("projectdata_state_id_fkey")
                    .OnDelete(DeleteBehavior.NoAction);

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

                entity.Property(e => e.OffsetX).HasColumnName("offset_x");
                entity.Property(e => e.OffsetY).HasColumnName("offset_y");
                entity.Property(e => e.OffsetZ).HasColumnName("offset_z");

            });

            modelBuilder.Entity<Line>(entity =>
            {
                entity.ToTable("line");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Handle).HasColumnName("handle");
                entity.Property(e => e.Layer).HasColumnName("layer");
                entity.Property(e => e.Correct).HasColumnName("correct");

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
                entity.Property(e => e.Correct).HasColumnName("correct");

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

            modelBuilder.Entity<Layer>(entity =>
            {
                entity.ToTable("layer");

                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.LineType).HasColumnName("linetype");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");
                entity.HasOne(e => e.Project)
                    .WithMany(e => e.Layers)
                    .HasForeignKey(e => e.ProjectId)
                    .HasConstraintName("layer_projectdata_id_fkey")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Seed();
            
        }
    }

}
