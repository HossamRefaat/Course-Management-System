using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class Lesson
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("Module")]
        public Guid ModuleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; }

        //if video => VideoUrl
        //if document => Document Content in html format
        //if quiz => null
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string InstructorId { get; set; }

        //Nav props
        public Quiz? Quiz { get; set; }
        public Module Module { get; set; }
    }
}
