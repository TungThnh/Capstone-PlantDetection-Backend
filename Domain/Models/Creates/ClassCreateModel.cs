namespace Domain.Models.Creates
{
    public class ClassCreateModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int NumberOfMember { get; set; }
    }
}
