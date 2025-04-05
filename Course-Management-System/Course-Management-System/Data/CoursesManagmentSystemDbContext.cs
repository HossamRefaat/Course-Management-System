using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Data
{
    public class CoursesManagmentSystemDbContext : DbContext
    {
        public CoursesManagmentSystemDbContext(DbContextOptions<CoursesManagmentSystemDbContext> options)
            : base(options)
        {
        }
    }
}
