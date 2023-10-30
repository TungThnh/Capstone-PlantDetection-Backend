namespace Domain.Models.Systems
{
    public class AuthModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
