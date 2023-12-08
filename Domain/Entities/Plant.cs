using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Plant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public string? Description { get; set; }

    public string? LivingCondition { get; set; }

    public string? DistributionArea { get; set; }

    public string? Ph { get; set; }

    public string? Uses { get; set; }

    public string? ScienceName { get; set; }

    public string? FruitTime { get; set; }

    public string? ConservationStatus { get; set; }

    public string? Size { get; set; }

    public string? Discoverer { get; set; }

    public string? Genus { get; set; }

    public string? Species { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<PlantCategory> PlantCategories { get; set; } = new List<PlantCategory>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();
}
