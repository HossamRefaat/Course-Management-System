using CourseManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class Course
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Instructor")]
        public string InstructorId { get; set; }

        //Nav props
        public ApplicationUser Instructor { get; set; }
        public ICollection<Module> Modules { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
