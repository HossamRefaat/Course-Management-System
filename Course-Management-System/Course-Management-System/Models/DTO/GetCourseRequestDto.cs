namespace Course_Management_System.Models.DTO
{
    public class GetCourseRequestDto
    {
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
