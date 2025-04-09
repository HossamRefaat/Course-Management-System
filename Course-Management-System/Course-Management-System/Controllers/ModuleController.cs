using AutoMapper;
using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Interfaces;
using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IModuleRepository moduleRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IMapper mapper;

        public ModuleController
        (
            UserManager<ApplicationUser> userManager,
            IModuleRepository moduleRepository,
            ICourseRepository courseRepository,
            IMapper mapper
        )
        {
            this.userManager = userManager;
            this.moduleRepository = moduleRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("{courseId:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create([FromRoute] Guid courseId,[FromBody] CreateModuleRequestDto module)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var courseModel = await courseRepository.GetCourseByIdAsync(courseId);
            if (courseModel == null) return NotFound("Course not found");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != courseModel.InstructorId) return Unauthorized();

            var moduleModel = new Module
            {
                Id = Guid.NewGuid(),
                Title = module.Title,
                CourseId = courseId,
                CreatedAt = DateTime.UtcNow,
                InstructorId = userId,
            };

            var createdModule = await moduleRepository.CreateModuleAsync(moduleModel);

            var moduleDto = mapper.Map<ModuleDto>(createdModule);

            return CreatedAtAction(nameof(GetById), new { moduleId = moduleDto.Id }, moduleDto);
        }

        [HttpGet("{moduleId:guid}")]
        public async Task<IActionResult> GetById(Guid moduleId)
        {
            var module = await moduleRepository.GetModuleByIdAsync(moduleId);
            if (module == null) return NotFound();
            var moduleDto = mapper.Map<ModuleDto>(module);

            return Ok(moduleDto);
        }

        [HttpGet("courses/{courseId:guid}")]
        public async Task<IActionResult> GetByCourseId(Guid courseId)
        {
            var modules = await moduleRepository.GetModulesByCourseIdAsync(courseId);
            if (modules == null) return NotFound();
            var modulesDto = mapper.Map<IEnumerable<ModuleDto>>(modules);
            return Ok(modulesDto);
        }

        [HttpPut("{moduleId:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateModule([FromRoute] Guid moduleId, [FromBody] UpdateModuleRequestDto request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var module = await moduleRepository.GetModuleByIdAsync(moduleId);
            if (module == null) return NotFound("Course not found");

            var userId = userManager.GetUserId(User);
            if (module.InstructorId != userId)
                return Forbid();

            module.Title = request.Title;

            var updatedModule = await moduleRepository.UpdateModuleAsync(module);

            if (updatedModule == null) return NotFound("Module not found");

            return Ok(updatedModule);
        }

        [HttpDelete("{moduleId:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteModule([FromRoute] Guid moduleId)
        {
            var module = await moduleRepository.GetModuleByIdAsync(moduleId);
            if (module == null) return NotFound("Course not found");

            var userId = userManager.GetUserId(User);
            if (module.InstructorId != userId)
                return Forbid();

            var state = await moduleRepository.DeleteModuleAsync(moduleId);
            if(!state) return NotFound();
            return Ok(state);
        }
    }
}
