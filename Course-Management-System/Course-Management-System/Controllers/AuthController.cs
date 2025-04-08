using CourseManagementSystem.API.Models;
using CourseManagementSystem.API.Models.DTO;
using CourseManagementSystem.API.Repositories;
using CourseManagementSystem.API.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourseManagementSystem.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Course_Management_System.Models.DTO;
using Microsoft.IdentityModel.Tokens;
using Course_Management_System.Data;

namespace CourseManagementSystem.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenRepository tokenRepository;
        private readonly IEmailSender emailSender;
        private readonly CoursesManagmentSystemDbContext dbcontext;

        public AuthController(UserManager<ApplicationUser> userManager,
            ITokenRepository tokenRepository, IEmailSender emailSender,
            CoursesManagmentSystemDbContext dbcontext)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.emailSender = emailSender;
            this.dbcontext = dbcontext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
        {
            var identityUser = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Name = registerDto.Name,
                Gender = registerDto.Gender,
                BirthDate = registerDto.BirthDate,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerDto.Password);

            if (identityResult.Succeeded)
            {
                if (registerDto.Roles != null && registerDto.Roles.Any())
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerDto.Roles);

                if (identityResult.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(identityUser);

                    await emailSender.SendEmailAsync(identityUser.Email, "Email Confirmation", $"Your confirmation code: {code}");

                    await dbcontext.ApplicationUsers.AddAsync(identityUser);
                    await dbcontext.SaveChangesAsync();

                    return Ok("You registered successfully. Check your email for the verification code.");

                }
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDto confirmEmailRequestDto)
        {
            if (string.IsNullOrWhiteSpace(confirmEmailRequestDto.UserId) || string.IsNullOrWhiteSpace(confirmEmailRequestDto.Code))
                return BadRequest("User ID and code are required.");

            var user = await userManager.FindByIdAsync(confirmEmailRequestDto.UserId);
            if (user == null)
                return NotFound("User not found.");

            var result = await userManager.ConfirmEmailAsync(user, confirmEmailRequestDto.Code);

            if (result.Succeeded)
                return Ok("Email confirmed successfully!");

            return BadRequest("Invalid or expired confirmation code.");
        }

        [Route("ForgetPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // Store the code temporarily
            await userManager.SetAuthenticationTokenAsync(user, "PasswordReset", "Code", token);

            await emailSender.SendEmailAsync(user.Email, "Password Reset Code", $"Your password reset code: {token}");

            return Ok(new { message = "Reset password code has been sent to your email" });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("Invalid email.");

            // Retrieve stored reset code
            var storedCode = await userManager.GetAuthenticationTokenAsync(user, "PasswordReset", "Code");
            if (storedCode != model.Code)
                return BadRequest("Invalid or expired reset code.");

            // Reset password
            var result = await userManager.ResetPasswordAsync(user, await userManager.GeneratePasswordResetTokenAsync(user), model.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Remove the stored reset code
            await userManager.RemoveAuthenticationTokenAsync(user, "PasswordReset", "Code");

            return Ok("Password has been reset successfully.");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);
            if(user != null)
            {
                var checkPassword = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPassword)
                { 
                    //get the user roles
                    var roles = await userManager.GetRolesAsync(user);

                    //Create Token
                    if (roles != null)
                    {
                        var tokenDto = await tokenRepository.CreateJWTToken(true, user, roles.ToList());
                        return Ok(tokenDto);
                    }
                }
            }

            return BadRequest("Username or password is wrong.");
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            if (tokenDto == null || string.IsNullOrEmpty(tokenDto.AccessToken) || string.IsNullOrEmpty(tokenDto.RefershToken))
            {
                return BadRequest("Invalid token data.");
            }

            try
            {
                var newTokens = await tokenRepository.RefreshToken(tokenDto);
                return Ok(newTokens);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }
        }

        [HttpGet("Me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (userId == null)
                return Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found");

            var roles = await userManager.GetRolesAsync(user);

            var result = new GetCurrentUserResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                Roles = roles,
            };

            return Ok(result);
        }

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserRequestDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized("User not identified");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            user.Name = dto.Name;   
            user.Gender = dto.Gender;
            user.BirthDate = dto.BirthDate;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Profile updated successfully" });
        }
    }
}
