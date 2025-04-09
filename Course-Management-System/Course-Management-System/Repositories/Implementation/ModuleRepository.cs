using Course_Management_System.Data;
using Course_Management_System.Models.Domain;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Course_Management_System.Repositories.Implementation
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly CoursesManagmentSystemDbContext dbContext;

        public ModuleRepository(CoursesManagmentSystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Module> CreateModuleAsync(Module module)
        {
            await dbContext.Modules.AddAsync(module);
            await dbContext.SaveChangesAsync();
            return module;
        }

        public async Task<bool> DeleteModuleAsync(Guid moduleId)
        {
            var module = await dbContext.Modules.FirstOrDefaultAsync(m => m.Id == moduleId);
            if (module == null) return false;
            dbContext.Modules.Remove(module);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Module?> GetModuleByIdAsync(Guid moduleId)
        {
            return await dbContext.Modules
                .FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid courseId)
        {
            return await dbContext.Modules
                .Where(m => m.CourseId == courseId)
                .ToListAsync();
        }

        public async Task<Module?> UpdateModuleAsync(Module module)
        {
            var existingModule = await dbContext.Modules.FirstOrDefaultAsync(m => m.Id == module.Id);
            if (existingModule == null)
            {
                return null;
            }

            existingModule.Title = module.Title;

            await dbContext.SaveChangesAsync();
            return existingModule;
        }
    }
}
