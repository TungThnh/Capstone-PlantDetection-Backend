using System;
using System.Collections.Generic;

namespace PlantDetection_Service.Domain.Entities

public partial class Result
{
    public Guid Id { get; set; }

    public Guid ReportId { get; set; }

    public Guid? PlantId { get; set; }

    public string? Message { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual Plant? Plant { get; set; }

    public virtual Report Report { get; set; } = null!;
}