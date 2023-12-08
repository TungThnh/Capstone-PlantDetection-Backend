using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Result
{
    public Guid Id { get; set; }

    public Guid ReportId { get; set; }

    public Guid? PlantId { get; set; }

    public string? Message { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Plant? Plant { get; set; }
}
