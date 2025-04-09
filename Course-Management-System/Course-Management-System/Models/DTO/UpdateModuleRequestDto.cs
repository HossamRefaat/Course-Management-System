using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class UpdateModuleRequestDto
    {
        [MaxLength(100)]
        public string Title { get; set; }
    }
}
