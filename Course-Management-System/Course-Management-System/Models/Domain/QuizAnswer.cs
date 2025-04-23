using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class QuizAnswer
    {
        public Guid Id { get; set; }
        public Guid QuizAttemptId { get; set; }

        [ForeignKey("Question")]
        public Guid QuizQuestionId { get; set; }

        public string SelectedAnswer { get; set; }
        public bool IsCorrect { get; set; }

        //nav prop
        public QuizQuestion Question { get; set; }
    }
}
