using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class UpdateLessonRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Content { get; set; }
    }
}
