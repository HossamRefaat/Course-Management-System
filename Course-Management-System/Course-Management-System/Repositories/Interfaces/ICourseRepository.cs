using Course_Management_System.Models.Domain;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>>? GetAllCoursesAsync();
        Task<Course>? GetCourseByIdAsync(Guid courseId);
        Task<IEnumerable<Course>>? GetCoursesByInstructorIdAsync(string instructorId);
        Task<Course> AddCourseAsync(Course course);
        Task<Course>? UpdateCourseAsync(Course course);
        Task<Course>? DeleteCourseAsync(Guid courseId);
    }
}
