namespace Quizera.WebUI
{
    public class AttemptIndexViewModel
    {
        public int AttemptId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string ExamTitle { get; set; } = string.Empty;
        public double? Score { get; set; }
        public double? TotalMarks { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }

}
