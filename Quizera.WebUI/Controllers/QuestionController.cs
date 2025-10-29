using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizera.BLL;
using Quizera.Domain;

namespace Quizera.WebUI.Controllers
{
    [Authorize(Roles = "INSTRUCTOR")]
    public class QuestionController : Controller
    {
        private readonly IGenericService<Question> _questionService;
        private readonly IGenericService<Option> _optionService;

        public QuestionController(
            IGenericService<Question> questionService,
            IGenericService<Option> optionService
            )
        {
            _questionService = questionService;
            _optionService = optionService;
        }

        #region GET : Question/Index
        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetAllWithIncludeAsync(q => q.Exam, q => q.Options);
            return View(questions);
        }

        #endregion

        #region  GET : Question/Details
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionService.GetByIdWithIncludeAsync(id, q => q.Exam, q => q.Options);
            if (question == null)
                return NotFound();

            return View(question);
        }
        #endregion

        #region GET: Question/Create
        public IActionResult Create(int examId)
        {
            var model = new CreateQuestionVM
            {
                ExamId = examId
            };

            // Optionally, start with 4 empty options
            for (int i = 0; i < 4; i++)
                model.Options.Add(new CreateOptionVM());

            return View(model);
        }
        #endregion

        #region POST: Question/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Step 1: Create Question
            var question = new Question
            {
                Text = model.Text,
                Marks = model.Marks,
                ExamId = model.ExamId
            };

            var createdQuestion = await _questionService.CreateAsync(question);

            // Step 2: Create Options for that Question
            foreach (var optionVM in model.Options)
            {
                if (string.IsNullOrWhiteSpace(optionVM.Text))
                    continue; // skip empty ones

                var option = new Option
                {
                    Text = optionVM.Text,
                    IsCorrect = optionVM.IsCorrect,
                    QuestionId = createdQuestion.Id
                };

                await _optionService.CreateAsync(option);
            }

            return RedirectToAction("Details", "Exam", new { id = model.ExamId });
        }
        #endregion

        #region GET: Question/Edit/5

        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionService.GetByIdAsync(id);

            var vm = new UpdateQuestionVM
            {
                Id = question.Id,
                Text = question.Text,
                Marks = question.Marks,
                ExamId = question.ExamId
            };

            return View(vm);
        }
        #endregion

        #region  POST: Question/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateQuestionVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingQuestion = await _questionService.GetByIdAsync(model.Id);

            var question = existingQuestion;
            question.Text = model.Text;
            question.Marks = model.Marks;
            question.ExamId = model.ExamId;

            var updateResult = await _questionService.UpdateAsync(question);
            if (updateResult == null)
            {
                return NotFound();
            }

            // Redirect back to Exam Details page
            return RedirectToAction("Details", "Exam", new { id = model.ExamId });
        }
        #endregion

        #region QustQuestion/Delete/id
        [HttpPost]
        public async Task<IActionResult> Delete(int id , int examId)
        {
            await _questionService.DeleteAsync(id);

            return RedirectToAction("Details", "Exam", new { id = examId });
        }

        #endregion
    }


}

