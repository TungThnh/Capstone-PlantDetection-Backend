using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class PlantCreateModel
    {
        public string Name { get; set; } = null!;

        public List<Guid> CategoryIds { get; set; } = null!;

        public string? Description { get; set; }

        public string Code { get; set; } = null!;

        public virtual ICollection<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
