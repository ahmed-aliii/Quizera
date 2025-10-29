namespace Quizera.WebUI
{
    public class AttemptSubmitViewModel
    {
        public int AttemptId { get; set; }

        // Key: QuestionId, Value: SelectedOptionId
        public Dictionary<int, int> Answers { get; set; } = new();
    }

}
