using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> Upload(Guid id, string contentType, IFormFile file);

        Task<string> Delete(Guid id);

        string GetMediaLink(Guid id);
    }
}
