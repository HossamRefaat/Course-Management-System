using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Repositories.Implementation
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly CoursesManagmentSystemDbContext dbContext;

        public EnrollmentRepository
        (
            CoursesManagmentSystemDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> EnrollInCourse(Course course, string studentId)
        {
            if(course is null) return false;

            var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                CourseId = course.Id,
                StudentId = studentId
            };

            await dbContext.Enrollments.AddAsync( enrollment );
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<Course>>? GetEnrollendCoursesAsync(string studentId) 
        {
            var courses = await dbContext.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .ThenInclude(c => c.Instructor) 
                .Select(e => e.Course!)
                .ToListAsync();

            return courses;
        }
    }
}
