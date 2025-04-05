using System.ComponentModel.DataAnnotations;

namespace Course_Management_System.Models.DTO
{
    public class GetCurrentUserResponseDto
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
