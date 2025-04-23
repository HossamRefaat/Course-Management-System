using Course_Management_System.Models.Domain;
using Course_Management_System.Models.DTO;
using Course_Management_System.Repositories.Implementation;
using Course_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
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


        [HttpPost("{id:guid}/attempts")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddAttempts([FromRoute] Guid id, AddQuizAnswerRequestDto request)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(id);
            if(quiz is null) return NotFound("Quiz not found");

            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var firstTime = await quizRepository.GetQuizAttemptByUserIdAsync(currentUser);
            if ((firstTime is not null) && (id == firstTime.QuizId))
                return BadRequest("You already submitted this quiz.");

            var quizAttempt = new QuizAttempt
            {
                Id = Guid.NewGuid(),
                QuizId = id,
                StudentId = currentUser,
                AttemptedAt = DateTime.Now
            };

            var quizAttemptRes = await quizRepository.AddQuizAttemptAsync(quizAttempt);
            if (!quizAttemptRes) return BadRequest();

            int correct = 0;
            foreach (var answer in request.Answers)
            {
                var question = await quizRepository.GetQuestionByIdAsync(answer.QuestionId);
                if (question == null) return NotFound("Question not found");

                var quizAnswer = new QuizAnswer
                {
                    Id = Guid.NewGuid(),
                    QuizAttemptId = quizAttempt.Id,
                    QuizQuestionId = answer.QuestionId,
                    SelectedAnswer = answer.SelectedAnswer,
                    IsCorrect = answer.SelectedAnswer == question.CorrectAnswer
                };

                var quizAnswerRes = await quizRepository.AddAnswerToQuizAttemptQuestionAsync(quizAnswer);
                if (!quizAnswerRes) return BadRequest();

                if (quizAnswer.IsCorrect) correct++;
            }

            var result = new
            {
                TotalQuestions = quiz.Questions.Count,
                CorrectAnswers = correct,
                Score = (double)correct / quiz.Questions.Count * 100
            };

            return Ok(result);
        }

        [HttpGet("{id:guid}/attempts")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> GetQuizAttempts([FromRoute] Guid id)
        {
            var quiz = await quizRepository.GetQuizByIdAsync(id);
            if (quiz is null) return NotFound("Quiz not found");
            var attempts = await quizRepository.GetQuizAttemptsByQuizIdAsync(id);
            var result = new List<GetAttemptRespone>();
            foreach (var attempt in attempts) 
            {
                var attemptRespone = new GetAttemptRespone()
                {
                    StudentName = attempt.Student.Name,
                    QuestionRespone = new List<QuestionAttemptRespone>()
                };

                foreach (var answer in attempt.Answers)
                {
                    var questionRespone = new QuestionAttemptRespone
                    {
                        Question = answer.Question.Question,
                        CorrectAnswer = answer.Question.CorrectAnswer,
                        StudentAnswer = answer.SelectedAnswer,
                    };

                    attemptRespone.QuestionRespone.Add(questionRespone);
                }

                result.Add(attemptRespone);
            }
            return Ok(result);
        }

        [HttpGet("quiz-attempts/{id:guid}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetQuizAttempt([FromRoute] Guid id)
        {
            var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUser is null) return Unauthorized();

            var attempt = await quizRepository.GetQuizAttemptsByQuizIdAndStudentId(id, currentUser);

            if (attempt is null) return BadRequest("You didn't take this exam before");

            var res = new List<QuestionAttemptRespone>();

            foreach(var answer in attempt.Answers)
            {
                res.Add
                (
                    new QuestionAttemptRespone
                    {
                        Question = answer.Question.Question,
                        StudentAnswer = answer.SelectedAnswer,
                        CorrectAnswer = answer.Question.CorrectAnswer
                    }
                );
            }

            return Ok(res);
        }
    }
}
