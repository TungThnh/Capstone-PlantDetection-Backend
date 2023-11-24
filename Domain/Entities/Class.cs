using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Class
{
    public Guid Id { get; set; }

    public Guid ManagerId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreateAt { get; set; }

    public int NumberOfMember { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public string ThumbnailUrl { get; set; } = null!;

    public virtual ICollection<ClassLabel> ClassLabels { get; set; } = new List<ClassLabel>();

    public virtual Manager Manager { get; set; } = null!;

    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}