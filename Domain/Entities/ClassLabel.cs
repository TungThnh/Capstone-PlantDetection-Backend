namespace Domain.Entities;

public partial class ClassLabel
{
    public Guid ClassId { get; set; }

    public Guid LabelId { get; set; }

    public string? Description { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Label Label { get; set; } = null!;
}