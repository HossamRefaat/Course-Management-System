using Course_Management_System.Models.Domain;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<List<Course>>? GetEnrollendCoursesAsync(string studentId);
        Task<bool> EnrollInCourse(Course course, string studentId);
    }
}
