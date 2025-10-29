using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class UpdateOptionWithinQuestionVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int ExamId { get; set; }
    }

}

