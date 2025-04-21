using Course_Management_System.Models.Domain;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> AddQuizAsync(Quiz quiz);
        Task<Quiz>? GetQuizByIdAsync(Guid id);
        Task<bool> UpdateQuizInfoOnlyAsync(Quiz quiz);
        Task<bool>? DeleteQuizAsync(Guid id);
        Task<bool>? AddQuestionToQuizAsync(QuizQuestion question);
        Task<QuizQuestion>? GetQuestionByIdAsync(Guid id);
        Task<bool>? UpdateQuestionAsync(QuizQuestion question);
        Task<bool>? DeleteQuestionAsync(Guid id);
        //Task<IEnumerable<Quiz>>? GetAllQuizzesAsync();
    }
}
