using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Exam
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public DateTime CreateAt { get; set; }

    public bool IsSubmitted { get; set; }

    public DateTime? SubmitAt { get; set; }

    public double? Score { get; set; }

    public virtual ICollection<QuestionExam> QuestionExams { get; set; } = new List<QuestionExam>();

    public virtual Student Student { get; set; } = null!;
}
