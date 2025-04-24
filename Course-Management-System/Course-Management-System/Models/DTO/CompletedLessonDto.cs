namespace Course_Management_System.Models.DTO
{
    public class CompletedLessonDto
    {
        public Guid LessonId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public DateTime CompletedAt { get; set; }
    }

}
