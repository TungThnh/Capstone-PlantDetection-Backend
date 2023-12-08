namespace Domain.Models.Views
{
    public class ExamResultViewModel
    {
        public Guid Id { get; set; }

        public double? Score { get; set; }

        public DateTime? SubmitAt { get; set; }

        public bool IsSubmitted { get; set; }

        public StudentViewModel Student { get; set; } = null!;

        public DateTime CreateAt { get; set; }

        public virtual ICollection<QuestionExamResultViewModel> QuestionExams { get; set; } = new List<QuestionExamResultViewModel>();
    }
}
