
namespace CourseManagementSystem.API.Helper
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receptor, string subject, string body);
    }
}