using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizera.BLL;
using Quizera.Domain;

namespace Quizera.WebUI.Controllers
{
    public class AttemptController : Controller
    {
        private readonly IExamService _examService;
        private readonly IAttemptService _attemptService;
        private readonly IGenericService<Answer> _answerService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AttemptController(
            IExamService examService,
            UserManager<ApplicationUser> userManager,
            IAttemptService attemptService,
            IGenericService<Answer> answerService
            )
        {
            _examService = examService;
            _userManager = userManager;
            _attemptService = attemptService;
            _answerService = answerService;
        }

        [Authorize(Roles = "INSTRUCTOR")]
        public async Task<IActionResult> Index()
        {

            // Get all attempts by this user
            var attempts = await _attemptService.GetAllWithIncludeAsync(a => a.Exam, a => a.Answers, a => a.Student);

            attempts = attempts.AsQueryable()
                .Include(a => a.Exam)
                    .ThenInclude(e => e.Questions)
                .Include(a => a.Student);

            var vm = attempts.Select(a => new AttemptIndexViewModel
            {
                AttemptId = a.Id,
                UserName = a.Student?.UserName ?? "Unknown User",
                ExamTitle = a.Exam?.Title ?? "Unknown Exam",
                Score = a.Score,
                TotalMarks = a.Exam != null ? a.Exam.Questions.Sum(q => q.Marks) : 0,
                StartedAt = a.StartedAt,
                FinishedAt = a.FinishedAt
            }).ToList();

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> MyAttempts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Get all attempts by this user
            var attempts = await _attemptService.GetAllWithIncludeAsync(a => a.Exam , a => a.Answers , a => a.Student);

            attempts = attempts.AsQueryable()
                .Include(a => a.Exam)
                    .ThenInclude(e => e.Questions)
                .Include(a => a.Student);

            attempts = attempts
                .Where(a => a.UserId == user.Id);

            // Map to ViewModel
            var vm = attempts.Select(a => new MyAttemptViewModel
            {
                AttemptId = a.Id,
                ExamTitle = a.Exam?.Title ?? "Unknown Exam",  // null-safe
                Score = a.Score != 0 ? a.Score : 0 ,                         // null-safe
                StartedAt = a.StartedAt,
                FinishedAt = a.FinishedAt,
                TotalMarks = a.Exam != null ? a.Exam.Questions.Sum(q => q.Marks) : 0 // null-safe

            }).ToList();

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Take(int id)
        {
            var attempt = await _attemptService.GetAllAttemptDetails(id);

            if (attempt == null) return NotFound();

            var vm = new AttemptTakeViewModel
            {
                AttemptId = attempt.Id,
                ExamTitle = attempt.Exam.Title,
                DurationInMinutes = attempt.Exam.DurationInMinutes,
                Questions = attempt.Exam.Questions.Select(q => new QuestionTakeViewModel
                {
                    QuestionId = q.Id,
                    Text = q.Text,
                    Marks = q.Marks,
                    Options = q.Options.Select(o => new OptionTakeViewModel
                    {
                        OptionId = o.Id,
                        Text = o.Text
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Submit(AttemptSubmitViewModel model)
        {
            var attempt = await _attemptService.GetAllAttemptDetails(model.AttemptId);

            if (attempt == null) return NotFound();

            double totalScore = 0;

            foreach (var entry in model.Answers)
            {
                var questionId = entry.Key;
                var selectedOptionId = entry.Value;

                var question = attempt.Exam.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question == null) continue;

                var selectedOption = question.Options.FirstOrDefault(o => o.Id == selectedOptionId);
                bool isCorrect = selectedOption != null && selectedOption.IsCorrect;

                if (isCorrect)
                    totalScore += question.Marks;

                await _answerService.CreateAsync(new Answer
                {
                    QuestionId = questionId,
                    OptionId = selectedOptionId,
                    AttemptId = attempt.Id,
                    IsCorrect = isCorrect
                });
            }

            attempt.Score = totalScore;
            attempt.FinishedAt = DateTime.UtcNow;

            await _attemptService.UpdateAsync(attempt);

            return RedirectToAction("Result", new { id = attempt.Id });
        }


        [HttpGet]
        public async Task<IActionResult> Result(int id)
        {
            var attempt = await _attemptService.GetAllAttemptDetails(id);
            if (attempt == null) return NotFound();

            var vm = new AttemptResultViewModel
            {
                ExamTitle = attempt.Exam.Title,
                Score = attempt.Score,
                TotalMarks = attempt.Exam.Questions.Sum(q => q.Marks),
            };

            return View(vm);
        }


    }
}
