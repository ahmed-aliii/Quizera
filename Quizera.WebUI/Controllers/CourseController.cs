using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizera.BLL;
using Quizera.Domain;
using System.Threading.Tasks;

namespace Quizera.WebUI.Controllers
{
    public class CourseController : Controller
    {
        private readonly IGenericService<Course> _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CourseController
            (
            IGenericService<Course> courseService,
             UserManager<ApplicationUser> userManager
            )
        {
            _courseService = courseService;
            _userManager = userManager;
        }


        #region Read
        public async Task<IActionResult> Index()
        {
           var courses = await _courseService.GetAllWithIncludeAsync(c => c.Instructor, c => c.Exams);
          
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetByIdWithIncludeAsync(id  , c => c.Instructor, c => c.Exams);

            return View(course);
        }

        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
            var model = new CreateCourseVM
            {
                Instructors = instructors.Select(i => new SelectListItem
                {
                    Value = i.Id,
                    Text = i.UserName
                })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                // reload instructors if validation fails
                var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
                model.Instructors = instructors.Select(i => new SelectListItem
                {
                    Value = i.Id,
                    Text = i.UserName
                });
                return View(model);
            }

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                InstructorId = model.InstructorId,
                CreatedAt = DateTime.UtcNow
            };

            await _courseService.CreateAsync(course);
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update
        // GET: /Course/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");

            var vm = new UpdateCourseVM
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                InstructorId = course.InstructorId,
                Instructors = instructors.Select(i => new SelectListItem
                {
                    Value = i.Id,
                    Text = i.UserName
                })
            };

            return View(vm);
        }

        // POST: /Course/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCourseVM model)
        {
            if (!ModelState.IsValid)
            {
                var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
                model.Instructors = instructors.Select(i => new SelectListItem
                {
                    Value = i.Id,
                    Text = i.UserName
                });

                return View(model);
            }

            var course = await _courseService.GetByIdAsync(model.Id);
            if (course == null)
                return NotFound();

            // Update entity fields
            course.Title = model.Title;
            course.Description = model.Description;
            course.InstructorId = model.InstructorId;

            await _courseService.UpdateAsync(course);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
                return NotFound();

            await _courseService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
