using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class CreateCourseRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
