namespace Quizera.WebUI
{
    public class StudentDetailsViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<StudentAttemptViewModel> Attempts { get; set; } = new();
    }

    public class StudentAttemptViewModel
    {
        public int AttemptId { get; set; }
        public string ExamTitle { get; set; } = string.Empty;
        public double Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
