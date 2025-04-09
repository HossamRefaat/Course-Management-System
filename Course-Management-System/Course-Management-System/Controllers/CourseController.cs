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
using System.Security.Claims;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly ICourseRepository courseRepository;

        public CourseController(UserManager<ApplicationUser> userManager, IMapper mapper, ICourseRepository courseRepository)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.courseRepository = courseRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var course = mapper.Map<Course>(request);
            course.Id = Guid.NewGuid();
            course.InstructorId = userId;

            var created = await courseRepository.AddCourseAsync(course);
            return Ok(created);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var courses = mapper.Map<IEnumerable<GetCourseRequestDto>>
                (await courseRepository.GetAllCoursesAsync());

            if (courses == null)
                return NotFound();

            return Ok(courses);
        }

        [HttpGet]
        [Route("{courseId:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid courseId)
        {
            var course = await courseRepository.GetCourseByIdAsync(courseId);
            if (course == null)
                return NotFound();
            var courseDto = mapper.Map<GetCourseRequestDto>(course);
            return Ok(courseDto);
        }

        [HttpGet("instructor/{instructorId}")]
        public async Task<IActionResult> GetByInstructorId([FromRoute] string instructorId)
        {
            var courses = await courseRepository.GetCoursesByInstructorIdAsync(instructorId);
            if (courses == null)
                return NotFound();
            var courseDtos = mapper.Map<IEnumerable<GetCourseRequestDto>>(courses);
            return Ok(courseDtos);
        }

        [HttpPut]
        [Route("{courseId:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Update([FromRoute] Guid courseId, [FromBody] UpdateCourseRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var course = await courseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
                return NotFound();

            if (course.InstructorId != userId)
                return Forbid();

            mapper.Map(request, course);

            var updated = await courseRepository.UpdateCourseAsync(course);

            return Ok();
        }

        [HttpDelete("{courseId:guid}")]
        [Authorize(Roles = "Admin, Instructor")]
        public async Task<IActionResult> DeleteCourse([FromRoute] Guid courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var course = await courseRepository.GetCourseByIdAsync(courseId);

            if (course == null)
                return NotFound();

            if (course.InstructorId != userId && !User.IsInRole("Admin"))
                return Forbid();

            var result = await courseRepository.DeleteCourseAsync(courseId);

            if (result == null)
                return BadRequest("Failed to delete the course.");

            return NoContent();  
        }


    }
}
