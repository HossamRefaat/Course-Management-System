namespace Course_Management_System.Models.DTO
{
    public class QuizQuestionDto
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public string CorrectAnswer { get; set; } = string.Empty;
    }
}
