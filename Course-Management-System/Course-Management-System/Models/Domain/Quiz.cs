using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class Quiz
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Lesson")]
        public Guid LessonId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //nav props
        public Lesson Lesson { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; }
    }
}
