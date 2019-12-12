﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ContosoApi.Models
{
    public partial class ContosouniversityContext : DbContext
    {
        public ContosouniversityContext()
        {
        }

        public ContosouniversityContext(DbContextOptions<ContosouniversityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseInstructor> CourseInstructors { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<VwCourseStudent> VwCourseStudents { get; set; }
        public virtual DbSet<VwCourseStudentCount> VwCourseStudentCounts { get; set; }
        public virtual DbSet<VwDepartmentCourseCount> VwDepartmentCourseCounts { get; set; }
        public virtual DbSet<DepartmentInsertResult> DepartmentInsertResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.DepartmentId)
                    .HasName("IX_DepartmentID");

                entity.Property(e => e.DepartmentId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_dbo.Course_dbo.Department_DepartmentID");
            });

            modelBuilder.Entity<CourseInstructor>(entity =>
            {
                entity.HasKey(e => new { e.CourseId, e.InstructorId })
                    .HasName("PK_dbo.CourseInstructor");

                entity.HasIndex(e => e.CourseId)
                    .HasName("IX_CourseID");

                entity.HasIndex(e => e.InstructorId)
                    .HasName("IX_InstructorID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.CourseInstructors)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_dbo.CourseInstructor_dbo.Course_CourseID");

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.CourseInstructors)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_dbo.CourseInstructor_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.InstructorId)
                    .HasName("IX_InstructorID");

                entity.Property(e => e.RowVersion).IsRowVersion();

                entity.HasOne(d => d.Instructor)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.InstructorId)
                    .HasConstraintName("FK_dbo.Department_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasIndex(e => e.CourseId)
                    .HasName("IX_CourseID");

                entity.HasIndex(e => e.StudentId)
                    .HasName("IX_StudentID");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_dbo.Enrollment_dbo.Course_CourseID");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK_dbo.Enrollment_dbo.Person_StudentID");
            });

            modelBuilder.Entity<OfficeAssignment>(entity =>
            {
                entity.HasKey(e => e.InstructorId)
                    .HasName("PK_dbo.OfficeAssignment");

                entity.HasIndex(e => e.InstructorId)
                    .HasName("IX_InstructorID");

                entity.Property(e => e.InstructorId).ValueGeneratedNever();

                entity.HasOne(d => d.Instructor)
                    .WithOne(p => p.OfficeAssignment)
                    .HasForeignKey<OfficeAssignment>(d => d.InstructorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_dbo.OfficeAssignment_dbo.Instructor_InstructorID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.Discriminator).HasDefaultValueSql("('Instructor')");
            });

            modelBuilder.Entity<VwCourseStudent>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCourseStudents");
            });

            modelBuilder.Entity<VwCourseStudentCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwCourseStudentCount");
            });

            modelBuilder.Entity<VwDepartmentCourseCount>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwDepartmentCourseCount");
            });

            modelBuilder.Entity<DepartmentInsertResult>(entity =>
            {
                entity.HasNoKey();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = this.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);
                Console.WriteLine("Status: {0}", entry.State);
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues.SetValues(new { IsDeleted = true });
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = this.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);
                Console.WriteLine("Status: {0}", entry.State);
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(new { DateModified = DateTime.Now });
                        break;
                    case EntityState.Deleted:
                        entry.CurrentValues.SetValues(new { IsDeleted = true });
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}