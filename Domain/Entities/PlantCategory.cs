namespace Domain.Entities;

public partial class PlantCategory
{
    public Guid CategoryId { get; set; }

    public Guid PlantId { get; set; }

    public string? Description { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Plant Plant { get; set; } = null!;
}
