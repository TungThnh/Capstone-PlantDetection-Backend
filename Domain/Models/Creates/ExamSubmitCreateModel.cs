namespace Domain.Models.Creates
{
    public class ExamSubmitCreateModel
    {
        public virtual ICollection<QuestionExamSubmitCreateModel> QuestionExams { get; set; } = new List<QuestionExamSubmitCreateModel>();
    }
}
