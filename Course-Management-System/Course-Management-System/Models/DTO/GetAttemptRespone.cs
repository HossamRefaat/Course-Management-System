namespace Course_Management_System.Models.DTO
{
    public class GetAttemptRespone
    {
        public string StudentName { get; set; }
        public List<QuestionAttemptRespone>? QuestionRespone { get; set; }
    }
}
