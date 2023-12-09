namespace Domain.Models.Creates
{
    public class QuestionExamSubmitCreateModel
    {
        public Guid QuestionId { get; set; }

        public string? SelectedAnswer { get; set; }
    }
}