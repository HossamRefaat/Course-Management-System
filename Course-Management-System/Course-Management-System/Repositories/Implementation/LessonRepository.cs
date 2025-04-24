using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        public async Task<bool> MakeLessonsCompletedAsync(Guid lessonId, string userId)
        {
            var lesson = await context.Lessons.FirstOrDefaultAsync(l => l.Id == lessonId);
            if (lesson == null) return false;
            var markCompleted = new CompletedLesson
            {
                Id = Guid.NewGuid(),
                StudentId = userId,
                LessonId = lessonId,
                CompletedAt = DateTime.Now
            };

            await context.CompletedLessons.AddAsync(markCompleted);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MakeLessonsUncompletedAsync(Guid lessonId, string userId)
        {
            var lesson = await context.CompletedLessons
                .Where(l => l.LessonId == lessonId)
                .FirstOrDefaultAsync();

            if (lesson is null || userId != lesson.StudentId) return false;

            context.CompletedLessons.Remove(lesson);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            context.Lessons.Update(lesson);
            await context.SaveChangesAsync();
        }

        public async Task<List<CompletedLesson>> GetCompletedLessonsByModuleAsync(Guid moduleId, string studentId)
        {
            return await context.CompletedLessons
                .Include(cl => cl.Lesson)
                .Where(cl => cl.Lesson.ModuleId == moduleId && cl.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<bool> IsLessonCompleted(Guid lessonId)
        {
            var isCompleted = await context.CompletedLessons
                .Where(l => l.LessonId == lessonId)
                .FirstOrDefaultAsync();

            return isCompleted is null ? false : true;
        }
    }
}
