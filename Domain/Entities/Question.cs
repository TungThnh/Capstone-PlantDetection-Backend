using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Question
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string AnswerA { get; set; } = null!;

    public string AnswerB { get; set; } = null!;

    public string AnswerC { get; set; } = null!;

    public string AnswerD { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public virtual ICollection<QuestionExam> QuestionExams { get; set; } = new List<QuestionExam>();
}
