namespace PlantDetection_Service.Domain.Entities

public partial class Plant
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string Code { get; set; }

    public string Status { get; set; }

    public DateTime CreateAt { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<PlantCategory> PlantCategories { get; set; } = new List<PlantCategory>();

    public virtual ICollection<Result> Results { get; set; } = new List<Results>();


}

