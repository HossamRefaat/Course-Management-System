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
        Task<bool>? AddAnswerToQuizAttemptQuestionAsync(QuizAnswer answer);
        Task<bool>? AddQuizAttemptAsync(QuizAttempt attempt);
        Task<QuizAttempt>? GetQuizAttemptByUserIdAsync(string id);
        Task <IEnumerable<QuizAttempt>>? GetQuizAttemptsByQuizIdAsync(Guid id);
        Task <QuizAttempt>? GetQuizAttemptsByQuizIdAndStudentId(Guid quizId, string studentId);
        //Task<IEnumerable<Quiz>>? GetAllQuizzesAsync();
    }
}
