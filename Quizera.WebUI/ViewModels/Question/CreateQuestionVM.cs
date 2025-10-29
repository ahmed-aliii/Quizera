using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class CreateQuestionVM
    {
        [Required]
        public string Text { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Marks must be greater than 0")]
        public int Marks { get; set; }

        [Required]
        public int ExamId { get; set; }

        public List<CreateOptionVM> Options { get; set; } = new List<CreateOptionVM>();
    }

    public class CreateOptionVM
    {
        [Required]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }

}
