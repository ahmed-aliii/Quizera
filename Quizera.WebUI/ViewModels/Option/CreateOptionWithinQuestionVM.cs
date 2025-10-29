using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class CreateOptionWithinQuestionVM
    {
        [Required]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int ExamId { get; set; }
    }
}
