namespace Quizera.WebUI
{
    public class MyAttemptViewModel
    {
        public int AttemptId { get; set; }
        public string ExamTitle { get; set; }
        public double Score { get; set; }
        public double TotalMarks { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }

}
