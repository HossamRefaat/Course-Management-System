using Course_Management_System.Repositories.Interfaces;
using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Course_Management_System.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> DeleteUserByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await Task.FromResult(userManager.Users.ToList());
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> SetUserRolesAsync(ApplicationUser user, List<string> newRoles)
        {
            var currentRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded) return removeResult;

            return await userManager.AddToRolesAsync(user, newRoles);
        }

    }
}
