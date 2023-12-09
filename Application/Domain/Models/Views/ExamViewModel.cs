namespace Domain.Models.Views
{
    public class ExamViewModel
    {
        public Guid Id { get; set; }

        public StudentViewModel Student { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public DateTime? SubmitAt { get; set; }

        public bool IsSubmitted { get; set; }

        public virtual ICollection<QuestionExamViewModel> QuestionExams { get; set; } = new List<QuestionExamViewModel>();
    }
}
