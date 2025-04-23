using CourseManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class QuizAttempt
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }

        [ForeignKey("Student")]
        public string StudentId { get; set; }

        public DateTime AttemptedAt { get; set; } = DateTime.Now;

        public ICollection<QuizAnswer> Answers { get; set; }
        public ApplicationUser Student { get; set; }
    }
}
