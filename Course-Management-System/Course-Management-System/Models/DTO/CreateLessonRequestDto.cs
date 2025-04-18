namespace Course_Management_System.Models.DTO
{
    public class CreateLessonRequestDto
    {
        public string Title { get; set; }
        public string Type { get; set; }
        public string? Content { get; set; }
    }
}
