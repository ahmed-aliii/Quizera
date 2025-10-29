using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quizera.BLL;
using Quizera.Domain;

namespace Quizera.WebUI.Controllers
{
    [Authorize(Roles = "INSTRUCTOR")]
    public class ExamController : Controller
    {
        private readonly IExamService _examService;
        private readonly IGenericService<Course> _courseService;
        private readonly IGenericService<Attempt> _attemptService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamController(
            IExamService examService,
            IGenericService<Course> courseService,
            UserManager<ApplicationUser> userManager,
            IGenericService<Attempt> attemptService
            )
        {
            _examService = examService;
            _courseService = courseService;
            _userManager = userManager;
            _attemptService = attemptService;
        }

        #region Read
        [AllowAnonymous]
        // INDEX ACTION
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var exams = await _examService.GetAllWithIncludeAsync(e => e.Course);

            return View(exams);
        }


        // DETAILS ACTION
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var exam = await _examService.GetDetailsByIdAsync(id);

            if (exam == null)
                return NotFound();

            return View(exam);
        }
        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var courses = await _courseService.GetAllAsync();

            var vm = new CreateExamVM
            {
                Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                })
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExamVM vm)
        {
            if (!ModelState.IsValid)
            {
                var courses = await _courseService.GetAllAsync();
                vm.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                });
                return View(vm);
            }

            var exam = new Exam
            {
                Title = vm.Title,
                Description = vm.Description,
                DurationInMinutes = vm.DurationInMinutes,
                CourseId = vm.CourseId
            };

            await _examService.CreateAsync(exam);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _examService.GetByIdAsync(id);
            if (exam == null)
                return NotFound();

            var courses = await _courseService.GetAllAsync();

            var vm = new UpdateExamVM
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Description,
                DurationInMinutes = exam.DurationInMinutes,
                CourseId = exam.CourseId,
                Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                    Selected = (c.Id == exam.CourseId)
                })
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateExamVM vm)
        {
            if (!ModelState.IsValid)
            {
                var courses = await _courseService.GetAllAsync();
                vm.Courses = courses.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title,
                    Selected = (c.Id == vm.CourseId)
                });
                return View(vm);
            }

            var exam = await _examService.GetByIdAsync(vm.Id);
            if (exam == null)
                return NotFound();

            exam.Title = vm.Title;
            exam.Description = vm.Description;
            exam.DurationInMinutes = vm.DurationInMinutes;
            exam.CourseId = vm.CourseId;

            await _examService.UpdateAsync(exam);
            return RedirectToAction("Index");
        }

        #endregion

        #region Start
        [AllowAnonymous]
        public async Task<IActionResult> Start(int Id)
        {
            var userId = _userManager.GetUserId(User);

            // Create Attempt
            var attempt = new Attempt
            {
                UserId = userId,
                ExamId = Id,
                StartedAt = DateTime.UtcNow
            };

            await _attemptService.CreateAsync(attempt);

            // Redirect to Attempt view (exam-taking page)
            return RedirectToAction("Take", "Attempt", new { id = attempt.Id });
        }

        #endregion
    }
}
