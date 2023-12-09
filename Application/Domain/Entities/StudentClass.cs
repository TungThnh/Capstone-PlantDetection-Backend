using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class StudentClass
{
    public Guid ClassId { get; set; }

    public Guid StudentId { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime? JoinAt { get; set; }

    public string Status { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
