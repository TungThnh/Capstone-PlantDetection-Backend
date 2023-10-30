namespace Domain.Entities;

public partial class Image
{
    public Guid Id { get; set; }

    public Guid PlantId { get; set; }

    public string Url { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public virtual Plant Plant { get; set; } = null!;
}
