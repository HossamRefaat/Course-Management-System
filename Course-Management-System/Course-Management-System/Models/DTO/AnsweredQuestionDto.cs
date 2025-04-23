using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class AnsweredQuestionDto
    {
        public Guid QuestionId { get; set; }

        [Required]
        public string SelectedAnswer { get; set; }
    }
}
