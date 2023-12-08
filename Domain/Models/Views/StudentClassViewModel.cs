namespace Domain.Models.Views
{
    public class StudentClassViewModel
    {
        public DateTime CreateAt { get; set; }

        public DateTime? JoinAt { get; set; }

        public string Status { get; set; } = null!;

        public string? Description { get; set; } = null!;

        public int Reports { get; set; }

        public virtual StudentViewModel Student { get; set; } = null!;
    }
}