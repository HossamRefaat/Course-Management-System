using Course_Management_System.Models.Domain;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface ILessonRepository
    {
        Task<Lesson> CreateLessonsByModuleIdAsync(Lesson lesson);
        Task<Lesson>? GetLessonByIdAsync(Guid lessonId);
        Task<IEnumerable<Lesson>>? GetLessonsByModuleIdAsync(Guid moduleId);
        Task<Lesson>? DeleteLessonByIdAsync(Guid lessonId);
        Task UpdateLessonAsync(Lesson lesson);
    }
}
