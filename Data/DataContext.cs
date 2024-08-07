using Microsoft.EntityFrameworkCore;
using SchoolApp.Models;

namespace SchoolApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TeacherStudent> TeacherStudents { get; set; }
        public DbSet<School> Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeacherStudent>()
                    .HasKey(st => new { st.StudentId, st.TeacherId });
            modelBuilder.Entity<TeacherStudent>()
                    .HasOne(st => st.Student)
                    .WithMany(s => s.TeacherStudents)
                    .HasForeignKey(st => st.StudentId);
            modelBuilder.Entity<TeacherStudent>()
                    .HasOne(st => st.Teacher)
                    .WithMany(t => t.TeacherStudents)
                    .HasForeignKey(st => st.TeacherId);
        }
    }
}
