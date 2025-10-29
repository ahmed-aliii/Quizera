using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizera.BLL;
using Quizera.Domain;

namespace Quizera.WebUI.Controllers
{
    [Authorize(Roles = "INSTRUCTOR")]
    public class OptionController : Controller
    {
        private readonly IGenericService<Option> _optionService;

        public OptionController(IGenericService<Option> optionService)
        {
            _optionService = optionService;
        }

        #region GET : Option/Create  
        public IActionResult Create(int questionId, int examId)
        {
            var vm = new CreateOptionWithinQuestionVM
            {
                QuestionId = questionId,
                ExamId = examId
            };
            return View(vm);
        }
        #endregion

        #region POST : Option/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOptionWithinQuestionVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var entity = new Option
            {
                Text = vm.Text,
                IsCorrect = vm.IsCorrect,
                QuestionId = vm.QuestionId
            };

            await _optionService.CreateAsync(entity);

            return RedirectToAction("Details", "Exam", new { id = vm.ExamId });
        }
        #endregion

        #region  GET: Option/Edit
        public async Task<IActionResult> Edit(int id, int questionId, int examId)
        {
            var option = await _optionService.GetByIdAsync(id);
            if (option == null)
                return NotFound();

            var vm = new UpdateOptionWithinQuestionVM
            {
                Id = option.Id,
                Text = option.Text,
                IsCorrect = option.IsCorrect,
                QuestionId = questionId,
                ExamId = examId
            };

            return View(vm);
        }
        #endregion

        #region Post: Option/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOptionWithinQuestionVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var option = await _optionService.GetByIdAsync(vm.Id);
            if (option == null)
                return NotFound();

            option.Text = vm.Text;
            option.IsCorrect = vm.IsCorrect;

            await _optionService.UpdateAsync(option);

            return RedirectToAction("Details", "Exam", new { id = vm.ExamId });

        }
        #endregion

        #region Get: Delete
        public async Task<IActionResult> Delete(int id , int examId)
        {
            var option = await _optionService.GetByIdAsync(id);

            if (option == null)
                return NotFound();

            await _optionService.DeleteAsync(id);

            return RedirectToAction("Details", "Exam", new { id = examId });

        }

        #endregion

    }
}
