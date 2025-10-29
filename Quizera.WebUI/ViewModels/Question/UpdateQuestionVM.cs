using System.ComponentModel.DataAnnotations;

namespace Quizera.WebUI
{
    public class UpdateQuestionVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Question text is required")]
        public string Text { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Marks must be greater than 0")]
        public int Marks { get; set; }

        [Required]
        public int ExamId { get; set; }
    }

}
