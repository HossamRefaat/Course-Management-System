using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class ModuleDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid CourseId { get; set; }
    }
}
