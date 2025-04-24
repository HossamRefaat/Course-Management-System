using CourseManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Management_System.Models.Domain
{
    public class CompletedLesson
    {
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string StudentId { get; set; }

        [ForeignKey("Lesson")]
        public Guid LessonId { get; set; }

        public DateTime CompletedAt { get; set; } = DateTime.Now;

        //nav props
        public ApplicationUser User { get; set; }
        public Lesson Lesson { get; set; }
    }
}
