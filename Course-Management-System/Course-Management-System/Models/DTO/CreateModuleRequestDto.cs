using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class CreateModuleRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
