namespace Course_Management_System.Models.DTO
{
    public class UpdateQuizQuestionDto
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
