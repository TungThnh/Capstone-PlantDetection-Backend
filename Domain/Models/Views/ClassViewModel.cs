namespace Domain.Models.Views
{
    public class ClassViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string? Description { get; set; }

        public string? Note { get; set; }

        public DateTime CreateAt { get; set; }

        public int NumberOfMember { get; set; }

        public string Status { get; set; } = null!;

        public ManagerViewModel Manager { get; set; } = null!;
    }
}
