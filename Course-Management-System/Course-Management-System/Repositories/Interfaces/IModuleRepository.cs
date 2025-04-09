using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;

namespace Course_Management_System.Repositories.Interfaces
{
    public interface IModuleRepository
    {
        Task<Module> CreateModuleAsync(Module module);
        Task<Module?> GetModuleByIdAsync(Guid moduleId);
        Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid courseId);
        Task<bool> DeleteModuleAsync(Guid moduleId);
        Task<Module?> UpdateModuleAsync(Module module);
    }
}
