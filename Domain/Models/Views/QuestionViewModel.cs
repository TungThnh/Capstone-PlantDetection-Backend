namespace Domain.Models.Views
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string AnswerA { get; set; } = null!;

        public string AnswerB { get; set; } = null!;

        public string AnswerC { get; set; } = null!;

        public string AnswerD { get; set; } = null!;
    }
}
