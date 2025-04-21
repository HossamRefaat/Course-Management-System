using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Repositories.Implementation
{
    public class LessonRepository : ILessonRepository
    {
        private readonly CoursesManagmentSystemDbContext context;

        public LessonRepository
        (
            CoursesManagmentSystemDbContext context
        )
        {
            this.context = context;
        }

        public async Task<Lesson> CreateLessonsByModuleIdAsync(Lesson lesson)
        {
            await context.Lessons.AddAsync(lesson);
            await context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson>? DeleteLessonByIdAsync(Guid lessonId)
        {
            var lesson = await context.Lessons.FindAsync(lessonId);
            if (lesson == null) return null;
            context.Lessons.Remove(lesson);
            await context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson>? GetLessonByIdAsync(Guid lessonId)
        {
            return await context.Lessons
                .Include(l => l.Quiz)
                .ThenInclude(q => q.Questions)
                .FirstOrDefaultAsync(l => l.Id == lessonId);
        }

        public async Task<IEnumerable<Lesson>>? GetLessonsByModuleIdAsync(Guid moduleId)
        {
            return await context.Lessons
                .Where(l => l.ModuleId == moduleId)
                .ToListAsync();
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            context.Lessons.Update(lesson);
            await context.SaveChangesAsync();
        }
    }
}
