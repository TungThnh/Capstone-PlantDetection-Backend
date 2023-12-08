using Microsoft.AspNetCore.Http;

namespace Domain.Models.Creates
{
    public class ClassModelCreateModel
    {
        public IFormFile Model {  get; set; } = null!;
    }
}
