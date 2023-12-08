using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class ClassCreateModel
    {
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;

        public ICollection<ClassLabelCreateModel> ClassLabels { get; set; } = null!;

        public string? Description { get; set; }

        public int NumberOfMember { get; set; }

        public IFormFile Thumbnail { get; set; } = null!;
    }
}
