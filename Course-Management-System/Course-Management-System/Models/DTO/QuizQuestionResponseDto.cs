namespace Course_Management_System.Models.DTO
{
        public class QuizQuestionResponseDto
        {
            public Guid Id { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public List<string> Options { get; set; }
        }
}
