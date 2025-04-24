using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser>? GetUserByIdAsync(string userId);
        Task<IList<string>>? GetUserRolesAsync(ApplicationUser user);
        Task<IdentityResult> SetUserRolesAsync(ApplicationUser user, List<string> newRoles);
        Task<bool> DeleteUserByIdAsync(string userId);
    }
}
