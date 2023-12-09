namespace Domain.Models.Filters
{
    public class ClassFilterModel
    {
        public Guid? ManagerId { get; set; }

        public Guid? StudentId { get; set; }

        public string? Name { get; set; } = null!;

        public string? Status { get; set; } = null!;
    }
}
