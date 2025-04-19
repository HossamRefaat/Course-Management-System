using CourseManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class Enrollment
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Student")]
        public string StudentId { get; set; }
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public DateTime EnrolledAt { get; set; } = DateTime.Now;

        //nav props
        public ApplicationUser Student { get; set; }
        public Course Course { get; set; }
    }
}
