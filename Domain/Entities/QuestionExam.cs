using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class QuestionExam
{
    public Guid QuestionId { get; set; }

    public Guid ExamId { get; set; }

    public string? Description { get; set; }

    public string? SelectedAnswer { get; set; }

    public virtual Exam Exam { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
