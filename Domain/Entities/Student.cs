namespace Domain.Entities;

public partial class Student
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AvatarUrl { get; set; } 

    public string? College { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? DayOfBirth { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual StudentClass? StudentClass { get; set; }
}
