using AutoMapper;
using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Implementation;
using Course_Management_System.Repositories.Interfaces;
using CourseManagementSystem.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository lessonRepository;
        private readonly IModuleRepository moduleRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public LessonController
        (
            ILessonRepository lessonRepository,
            IModuleRepository moduleRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper
        )
        {
            this.lessonRepository = lessonRepository;
            this.moduleRepository = moduleRepository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpPost("/api/modules/{moduleId}/lessons")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create(Guid moduleId, [FromBody] CreateLessonRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var module = await moduleRepository.GetModuleByIdAsync(moduleId);

            if (module == null) return NotFound("Module not found");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null || userId != module.InstructorId) return Unauthorized();


            var lesson = mapper.Map<Lesson>(request);
            lesson.Id = Guid.NewGuid();
            lesson.ModuleId = moduleId;
            lesson.CreatedAt = DateTime.Now;
            lesson.InstructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var createdLesson = await lessonRepository.CreateLessonsByModuleIdAsync(lesson);
            return CreatedAtAction(nameof(Get), new { id = createdLesson.Id }, createdLesson);
        }

        [HttpGet("/api/modules/{moduleId}/lessons")]
        public async Task<IActionResult> GetAll(Guid moduleId)
        {
            var lessons = await lessonRepository.GetLessonsByModuleIdAsync(moduleId);
            if (lessons == null) return NotFound();
            return Ok(lessons);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateLessonAsync(Guid id, [FromBody] UpdateLessonRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingLesson = await lessonRepository.GetLessonByIdAsync(id);
            if (existingLesson == null) return NotFound("Lesson not found");

            var userId = userManager.GetUserId(User);
            if (existingLesson.InstructorId != userId)
                return Forbid();

            if (existingLesson == null)
                return NotFound("Lesson not found.");

            // Use AutoMapper to map new values to the existing entity
            mapper.Map(request, existingLesson);

            await lessonRepository.UpdateLessonAsync(existingLesson);

            return Ok(existingLesson);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingLesson = await lessonRepository.GetLessonByIdAsync(id);
            if (existingLesson == null) return NotFound("Lesson not found");

            var userId = userManager.GetUserId(User);
            if (existingLesson.InstructorId != userId)
                return Forbid();

            var lesson = await lessonRepository.DeleteLessonByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var lesson = await lessonRepository.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound();
            return Ok(lesson);
        }

        [HttpPost("{id:guid}/progress")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MarkLessonCompleted([FromRoute] Guid id)
        {
            var userId = userManager.GetUserId(User);
            var isCompleted = await lessonRepository.IsLessonCompleted(id);
            if (!isCompleted)
            {
                var lessonsCompleleted = await lessonRepository
                .MakeLessonsCompletedAsync(id, userId);

                if (lessonsCompleleted) return Ok(lessonsCompleleted);
            }
             
            return BadRequest("Lesson not found or is already completed");
        }

        [HttpDelete("{id:guid}/progress")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MarkLessonUncompleted([FromRoute] Guid id)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null) return Unauthorized();
            return await lessonRepository.MakeLessonsUncompletedAsync(id, userId) ? Ok() : BadRequest("Error accured");
        }

        [HttpGet("{moduleId:guid}/completed-lessons")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetCompletedLessonsForModule(Guid moduleId)
        {
            var studentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var completedLessons = await lessonRepository.GetCompletedLessonsByModuleAsync(moduleId, studentId);

            var result = completedLessons.Select(cl => new CompletedLessonDto
            {
                LessonId = cl.Lesson.Id,
                Title = cl.Lesson.Title,
                Type = cl.Lesson.Type,
                CompletedAt = cl.CompletedAt
            }).ToList();

            return Ok(result);
        }

    }
}
