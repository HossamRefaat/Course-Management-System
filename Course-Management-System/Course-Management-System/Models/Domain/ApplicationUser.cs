using Course_Management_System.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.API.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]  
    public string Name { get; set; }

    [Required]
    [MaxLength(10)]  
    public string Gender { get; set; }

    [Required]
    [DataType(DataType.Date)]  
    public DateTime BirthDate { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
}

