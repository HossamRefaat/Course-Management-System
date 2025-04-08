using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Repositories.Implementation
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CoursesManagmentSystemDbContext dbContext;

        public CourseRepository(CoursesManagmentSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            await dbContext.Courses.AddAsync(course);
            await dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<Course>? DeleteCourseAsync(Guid courseId)
        {
            var course = await dbContext.Courses.FindAsync(courseId);
            if (course == null) return null;

            dbContext.Courses.Remove(course);
            await dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<IEnumerable<Course>>? GetAllCoursesAsync()
        {
            return await dbContext.Courses
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<Course>? GetCourseByIdAsync(Guid  courseId)
        {
            return await dbContext.Courses
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == courseId);   
        }

        public async Task<IEnumerable<Course>>? GetCoursesByInstructorIdAsync(string instructorId)
        {
            return await dbContext.Courses
                .Where(c => c.InstructorId == instructorId)
                .Include(c => c.Instructor)
                .ToListAsync();
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            dbContext.Courses.Update(course);
            await dbContext.SaveChangesAsync();
            return course;
        }
    }
}
