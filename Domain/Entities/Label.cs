using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Label
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ClassLabel> ClassLabels { get; set; } = new List<ClassLabel>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
