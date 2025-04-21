using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Implementation;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Course_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository quizRepository;
        private readonly ILessonRepository lessonRepository;

        public QuizController
        (
           IQuizRepository quizRepository,
           ILessonRepository lessonRepository
        )
        {
            this.quizRepository = quizRepository;
            this.lessonRepository = lessonRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Fetch the lesson by ID and ensure it's a valid quiz lesson type
            var lesson = await lessonRepository.GetLessonByIdAsync(request.LessonId);
            if (lesson == null) return NotFound("Lesson not found");

            if (lesson.InstructorId != userId) return Unauthorized();

            if (lesson.Type != "Quiz") return BadRequest("Lesson type must be 'Quiz'");

            // Create the Quiz entity
            var quiz = new Quiz
            {
                Id = Guid.NewGuid(),
                LessonId = request.LessonId,
                CreatedAt = DateTime.UtcNow
            };

            // Save the quiz and its questions
            var res = await quizRepository.AddQuizAsync(quiz);

            return Ok(res);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateQuizInfo([FromRoute] Guid id, [FromBody] UpdateQuizInfoDto request)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(id);
            if (quiz == null) return NotFound("Quiz not found");

            var lesson = await lessonRepository.GetLessonByIdAsync(request.LessonId);
            if (lesson == null || lesson.Type != "Quiz") return BadRequest("Invalid lesson");

            quiz.LessonId = request.LessonId;

            var updated = await quizRepository.UpdateQuizInfoOnlyAsync(quiz);
            return updated ? Ok("Quiz updated") : StatusCode(500, "Update failed");
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetQuiz([FromRoute] Guid id)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(id);
            return quiz is not null ? Ok(quiz) : NotFound();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteQuiz([FromRoute] Guid id)
        {
            return await quizRepository.DeleteQuizAsync(id) ? Ok() : NotFound();
        }

        [HttpPost("{id:guid}/questions")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> CreateQuestion([FromRoute] Guid id, [FromBody] CreateQuestionRequestDto request)
        {
            var question = new QuizQuestion
            {
                Id = Guid.NewGuid(),
                Question = request.Question,
                CorrectAnswer = request.CorrectAnswer,
                Options = request.Options,
                QuizId = id
            };

            var res = await quizRepository.AddQuestionToQuizAsync(question);
            return res ? Ok(res) : NotFound();
        }

        [HttpPut("{id:guid}/questions")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateQuizQuestions([FromRoute] Guid id, [FromBody] UpdateQuizQuestionDto request)
        {
            var question = await quizRepository.GetQuestionByIdAsync(id);
            if (question == null) return NotFound("Quiz not found");

            question.Question = request.Question;
            question.CorrectAnswer = request.CorrectAnswer;
            question.Options = request.Options;
            
            var updated = await quizRepository.UpdateQuestionAsync(question);
            return updated ? Ok("Questions updated") : StatusCode(500, "Failed to update questions");
        }

        [HttpDelete("{id:guid}/questions")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteQuizQuestion([FromRoute] Guid id)
        {
            return await quizRepository.DeleteQuestionAsync(id)? Ok() : NotFound();
        }
    }
}
