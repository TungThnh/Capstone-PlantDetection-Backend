using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class ReportCreateModel
    {
        public IFormFile Image { get; set; } = null!;

        public Guid LabelId { get; set; }

        public string? Description { get; set; }
    }
}
