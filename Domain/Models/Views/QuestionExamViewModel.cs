namespace Domain.Models.Views
{
    public class QuestionExamViewModel
    {
        public QuestionViewModel Question { get; set; } = null!;

        public string? SelectedAnswer { get; set; }

        public string? Description { get; set; }
    }
}
