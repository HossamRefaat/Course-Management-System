namespace Course_Management_System.Models.DTO
{
    public class QuizResponseDto
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }
        public List<QuizQuestionResponseDto> Questions { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
