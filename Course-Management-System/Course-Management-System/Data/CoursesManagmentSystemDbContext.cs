using Course_Management_System.Models.Domain;
using CourseManagementSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Data
{
    public class CoursesManagmentSystemDbContext : DbContext
    {
        public CoursesManagmentSystemDbContext(DbContextOptions<CoursesManagmentSystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Module> Modules { get; set; }
    }
}
