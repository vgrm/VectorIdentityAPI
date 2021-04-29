﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vector_control_system_api.Database;

namespace vector_control_system_api.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20210429013202_AddOffset")]
    partial class AddOffset
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("vector_control_system_api.Database.Arc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AngleEnd")
                        .HasColumnType("float")
                        .HasColumnName("angle_end");

                    b.Property<double>("AngleStart")
                        .HasColumnType("float")
                        .HasColumnName("angle_start");

                    b.Property<bool>("Correct")
                        .HasColumnType("bit")
                        .HasColumnName("correct");

                    b.Property<double>("DX")
                        .HasColumnType("float")
                        .HasColumnName("dx");

                    b.Property<double>("DY")
                        .HasColumnType("float")
                        .HasColumnName("dy");

                    b.Property<double>("DZ")
                        .HasColumnType("float")
                        .HasColumnName("dz");

                    b.Property<string>("Handle")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("handle");

                    b.Property<string>("Layer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("layer");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("project_id");

                    b.Property<double>("Radius")
                        .HasColumnType("float")
                        .HasColumnName("radius");

                    b.Property<double>("X")
                        .HasColumnType("float")
                        .HasColumnName("x");

                    b.Property<double>("Y")
                        .HasColumnType("float")
                        .HasColumnName("y");

                    b.Property<double>("Z")
                        .HasColumnType("float")
                        .HasColumnName("z");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("arc");
                });

            modelBuilder.Entity("vector_control_system_api.Database.Layer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LineType")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("linetype");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("project_id");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("layer");
                });

            modelBuilder.Entity("vector_control_system_api.Database.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Correct")
                        .HasColumnType("bit")
                        .HasColumnName("correct");

                    b.Property<double>("DX")
                        .HasColumnType("float")
                        .HasColumnName("dx");

                    b.Property<double>("DY")
                        .HasColumnType("float")
                        .HasColumnName("dy");

                    b.Property<double>("DZ")
                        .HasColumnType("float")
                        .HasColumnName("dz");

                    b.Property<string>("Handle")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("handle");

                    b.Property<string>("Layer")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("layer");

                    b.Property<double>("Magnitude")
                        .HasColumnType("float")
                        .HasColumnName("magnitude");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int")
                        .HasColumnName("project_id");

                    b.Property<double>("X1")
                        .HasColumnType("float")
                        .HasColumnName("x1");

                    b.Property<double>("X2")
                        .HasColumnType("float")
                        .HasColumnName("x2");

                    b.Property<double>("Y1")
                        .HasColumnType("float")
                        .HasColumnName("y1");

                    b.Property<double>("Y2")
                        .HasColumnType("float")
                        .HasColumnName("y2");

                    b.Property<double>("Z1")
                        .HasColumnType("float")
                        .HasColumnName("z1");

                    b.Property<double>("Z2")
                        .HasColumnType("float")
                        .HasColumnName("z2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("line");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime")
                        .HasColumnName("date_updated");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("datetime")
                        .HasColumnName("date_uploaded");

                    b.Property<byte[]>("FileData")
                        .HasColumnType("varbinary(max)")
                        .HasColumnName("file_data");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("file_type");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<double>("OffsetX")
                        .HasColumnType("float")
                        .HasColumnName("offset_x");

                    b.Property<double>("OffsetY")
                        .HasColumnType("float")
                        .HasColumnName("offset_y");

                    b.Property<double>("OffsetZ")
                        .HasColumnType("float")
                        .HasColumnName("offset_z");

                    b.Property<bool>("Original")
                        .HasColumnType("bit")
                        .HasColumnName("original");

                    b.Property<int>("OriginalProjectId")
                        .HasColumnType("int")
                        .HasColumnName("original_project_id");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int")
                        .HasColumnName("owner_id");

                    b.Property<int>("ProjectSetId")
                        .HasColumnType("int")
                        .HasColumnName("projectset_id");

                    b.Property<double>("ScoreCorrectness")
                        .HasColumnType("float")
                        .HasColumnName("score_correctness");

                    b.Property<double>("ScoreIdentity")
                        .HasColumnType("float")
                        .HasColumnName("score_identity");

                    b.Property<int>("StateId")
                        .HasColumnType("int")
                        .HasColumnName("state_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProjectSetId");

                    b.HasIndex("StateId");

                    b.ToTable("projectdata");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int")
                        .HasColumnName("owner_id");

                    b.Property<int>("StateId")
                        .HasColumnType("int")
                        .HasColumnName("state_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("StateId");

                    b.ToTable("projectset");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectSetState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("projectset_state");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Open"
                        },
                        new
                        {
                            Id = -2,
                            Name = "Closed"
                        },
                        new
                        {
                            Id = -3,
                            Name = "Private"
                        });
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("projectdata_state");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "New"
                        },
                        new
                        {
                            Id = -2,
                            Name = "Processing"
                        },
                        new
                        {
                            Id = -3,
                            Name = "Analyzing"
                        },
                        new
                        {
                            Id = -4,
                            Name = "Analyzed"
                        });
                });

            modelBuilder.Entity("vector_control_system_api.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("lastname");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password_salt");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("role_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("user_data_username_key");

                    b.ToTable("user_data");
                });

            modelBuilder.Entity("vector_control_system_api.Database.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.ToTable("user_role");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = -2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("vector_control_system_api.Database.Arc", b =>
                {
                    b.HasOne("vector_control_system_api.Database.ProjectData", "Project")
                        .WithMany("Arcs")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("arc_projectdata_id_fkey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("vector_control_system_api.Database.Layer", b =>
                {
                    b.HasOne("vector_control_system_api.Database.ProjectData", "Project")
                        .WithMany("Layers")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("layer_projectdata_id_fkey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("vector_control_system_api.Database.Line", b =>
                {
                    b.HasOne("vector_control_system_api.Database.ProjectData", "Project")
                        .WithMany("Lines")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("line_projectdata_id_fkey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectData", b =>
                {
                    b.HasOne("vector_control_system_api.Database.User", "Owner")
                        .WithMany("Projects")
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("projectdata_owner_id_fkey")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.HasOne("vector_control_system_api.Database.ProjectSet", "ProjectSet")
                        .WithMany("Projects")
                        .HasForeignKey("ProjectSetId")
                        .HasConstraintName("projectdata_projectset_id_fkey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vector_control_system_api.Database.ProjectState", "State")
                        .WithMany("Projects")
                        .HasForeignKey("StateId")
                        .HasConstraintName("projectdata_state_id_fkey")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("ProjectSet");

                    b.Navigation("State");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectSet", b =>
                {
                    b.HasOne("vector_control_system_api.Database.User", "Owner")
                        .WithMany("ProjectSets")
                        .HasForeignKey("OwnerId")
                        .HasConstraintName("projectset_owner_id_fkey")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("vector_control_system_api.Database.ProjectSetState", "State")
                        .WithMany("ProjectSets")
                        .HasForeignKey("StateId")
                        .HasConstraintName("projectset_state_id_fkey")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("State");
                });

            modelBuilder.Entity("vector_control_system_api.Database.User", b =>
                {
                    b.HasOne("vector_control_system_api.Database.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("user_data_role_id_fkey")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectData", b =>
                {
                    b.Navigation("Arcs");

                    b.Navigation("Layers");

                    b.Navigation("Lines");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectSet", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectSetState", b =>
                {
                    b.Navigation("ProjectSets");
                });

            modelBuilder.Entity("vector_control_system_api.Database.ProjectState", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("vector_control_system_api.Database.User", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("ProjectSets");
                });

            modelBuilder.Entity("vector_control_system_api.Database.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
