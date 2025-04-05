using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Data
{
    public class CoursesManagmentSystemAuthDbContext : IdentityDbContext
    {
        public CoursesManagmentSystemAuthDbContext(DbContextOptions<CoursesManagmentSystemAuthDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "6667913c-a3d0-4d20-b5bb-018ac670758e";
            var instructorRoleId = "32048ff8-4876-474b-b6b1-542818991943";
            var studentRoleId = "864541ce-8060-4ab1-9448-2720d9ba8be0";

            var roles = new List<IdentityRole>
            { 
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = instructorRoleId,
                    Name = "Instructor",
                    NormalizedName = "INSTRUCTOR"
                },
                new IdentityRole
                {
                    Id = studentRoleId,
                    Name = "Student",
                    NormalizedName = "STUDENT"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
