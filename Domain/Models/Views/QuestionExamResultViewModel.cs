namespace Domain.Models.Views
{
    public class QuestionExamResultViewModel
    {
        public QuestionResultViewModel Question { get; set; } = null!;

        public string? SelectedAnswer { get; set; }

        public string? Description { get; set; }
    }
}
