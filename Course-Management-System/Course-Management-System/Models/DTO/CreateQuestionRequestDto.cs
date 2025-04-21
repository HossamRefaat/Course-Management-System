using Course_Management_System.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class CreateQuestionRequestDto
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }   
}
