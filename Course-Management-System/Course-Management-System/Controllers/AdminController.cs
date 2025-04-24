using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Implementation;
using Course_Management_System.Repositories.Interfaces;
using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;

        public AdminController
        (
            UserManager<ApplicationUser> userManager,
            IUserRepository userRepository
        )
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userRepository.GetAllUsersAsync();
            var result = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.PhoneNumber
            });


            return Ok(result);
        }

        [HttpPut("{userId}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRoles([FromRoute] string userId, [FromBody] UpdateUserRolesDto dto)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user is null) return NotFound("User not found");

            if (dto.Roles == null || !dto.Roles.Any())
                return BadRequest("At least one role must be provided.");

            var result = await userRepository.SetUserRolesAsync(user, dto.Roles);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User roles updated successfully.");
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == currentUserId) return BadRequest("You cannot delete yourself");
            
            var success = await userRepository.DeleteUserByIdAsync(userId);
            if (!success)
                return NotFound("User not found or deletion failed.");

            return Ok("User deleted successfully.");
        }

    }
}
