using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class UplaodVideoRequestDtocs
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }
        public string? Description { get; set; }
    }
}
