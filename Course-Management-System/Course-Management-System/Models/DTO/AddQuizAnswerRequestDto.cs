using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class AddQuizAnswerRequestDto
    {
        [Required]
        public List<AnsweredQuestionDto> Answers { get; set; }
    }
}
