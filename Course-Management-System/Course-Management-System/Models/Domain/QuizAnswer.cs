namespace Course_Management_System.Models.Domain
{
    public class QuizAnswer
    {
        public Guid Id { get; set; }
        public Guid QuizAttemptId { get; set; }
        public Guid QuizQuestionId { get; set; }

        public string SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
