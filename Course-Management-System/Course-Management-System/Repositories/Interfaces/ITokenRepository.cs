using Course_Management_System.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace CourseManagementSystem.API.Repositories
{
    public interface ITokenRepository
    {
        Task<TokenDto> CreateJWTToken(bool populateExp, IdentityUser user, List<string> roles);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);   
    }
}
