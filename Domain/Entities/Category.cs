using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Category
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PlantCategory> PlantCategories { get; set; } = new List<PlantCategory>();
}
