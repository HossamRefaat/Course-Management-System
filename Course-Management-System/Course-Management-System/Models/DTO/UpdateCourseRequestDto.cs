using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class UpdateCourseRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
