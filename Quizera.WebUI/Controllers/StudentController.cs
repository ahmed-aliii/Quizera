using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizera.BLL;
using Quizera.Domain;

namespace Quizera.WebUI.Controllers
{
    [Authorize(Roles = "INSTRUCTOR")]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAttemptService _attemptService;

        public StudentController(UserManager<ApplicationUser> userManager, IAttemptService attemptService)
        {
            _userManager = userManager;
            _attemptService = attemptService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await _userManager.GetUsersInRoleAsync("STUDENT");

            var attempts = await _attemptService.GetAllWithIncludeAsync(
                a => a.Exam,
                a => a.Answers,
                a => a.Student
            );

            var vm = students.Select(u => new StudentIndexViewModel
            {
                UserId = u.Id,
                UserName = u.UserName ?? "Unknown",
                Email = u.Email ?? "No Email",
                TotalAttempts = attempts.Count(a => a.UserId == u.Id)
            }).ToList();

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var student = await _userManager.FindByIdAsync(id);
            if (student == null)
                return NotFound();

            var attempts = await _attemptService.GetAllWithIncludeAsync(a => a.Exam , a => a.Student);


            var vm = new StudentDetailsViewModel
            {
                UserId = student.Id,
                UserName = student.UserName ?? "Unknown",
                Email = student.Email ?? "No Email",
                Attempts = attempts.Select(a => new StudentAttemptViewModel
                {
                    AttemptId = a.Id,
                    ExamTitle = a.Exam?.Title ?? "Unknown Exam",
                    Score = a.Score,
                    StartedAt = a.StartedAt,
                    FinishedAt = a.FinishedAt
                }).ToList()
            };

            return View(vm);
        }

    }
}
