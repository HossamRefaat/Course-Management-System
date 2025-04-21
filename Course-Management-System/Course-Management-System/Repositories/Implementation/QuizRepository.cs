using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Course_Management_System.Repositories.Implementation
{
    public class QuizRepository : IQuizRepository
    {
        private readonly CoursesManagmentSystemDbContext context;

        public QuizRepository(CoursesManagmentSystemDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<bool> AddQuestionToQuizAsync(QuizQuestion question)
        {
            var quizExists = await context.Quizzes.AnyAsync(q => q.Id == question.QuizId);
            if (!quizExists) return false;

            question.QuizId = question.QuizId; 

            await context.QuizQuestions.AddAsync(question);
            await context.SaveChangesAsync();

            return true;
        }


        public async Task<Quiz> AddQuizAsync(Quiz quiz)
        {
            await context.Quizzes.AddAsync(quiz);
            await context.SaveChangesAsync();
            return quiz;
        }

        public async Task<bool>? DeleteQuestionAsync(Guid id)
        {
            var question = await context.QuizQuestions.FirstOrDefaultAsync(q => q.Id == id);
            if (question == null) return false;

            context.QuizQuestions.Remove(question);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteQuizAsync(Guid id)
        {
            var quiz = await context.Quizzes.FindAsync(id);
            if (quiz == null) return false;

            context.Quizzes.Remove(quiz);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<QuizQuestion>? GetQuestionByIdAsync(Guid id)
        {
            return await context.QuizQuestions
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quiz?> GetQuizByIdAsync(Guid id)
        {
            return await context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);
        }
        
        public async Task<bool>? UpdateQuestionAsync(QuizQuestion question)
        {
            var existingQuestion = await context.QuizQuestions.FirstOrDefaultAsync(q => q.Id == question.Id);
            if (existingQuestion == null) return false;

            existingQuestion.Question = question.Question;
            existingQuestion.Options = question.Options;
            existingQuestion.CorrectAnswer = question.CorrectAnswer;

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateQuizInfoOnlyAsync(Quiz quiz)
        {
            var existingQuiz = await context.Quizzes.FirstOrDefaultAsync(q => q.Id == quiz.Id);
            if (existingQuiz == null) return false;

            existingQuiz.LessonId = quiz.LessonId;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
