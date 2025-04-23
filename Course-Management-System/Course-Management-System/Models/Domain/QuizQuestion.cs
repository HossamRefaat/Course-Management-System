using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.Domain
{
    public class QuizQuestion
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<string> Options { get; set; } // Use ValueConverter for EF
        public string CorrectAnswer { get; set; }
        public Guid QuizId { get; set; }

        public Quiz Quiz { get; set; }
    }
}
