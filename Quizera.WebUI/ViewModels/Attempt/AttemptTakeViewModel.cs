namespace Quizera.WebUI
{
    public class AttemptTakeViewModel
    {
        public int AttemptId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public int DurationInMinutes { get; set; }

        public List<QuestionTakeViewModel> Questions { get; set; } = new();
    }

    public class QuestionTakeViewModel
    {
        public int QuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public double Marks { get; set; }

        public List<OptionTakeViewModel> Options { get; set; } = new();
    }

    public class OptionTakeViewModel
    {
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
    }

}
