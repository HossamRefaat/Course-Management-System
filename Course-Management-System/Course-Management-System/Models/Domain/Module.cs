using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class Module
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public string InstructorId { get; set; }

        //Nav props
        public Course Course { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
