using Domain.Models.Creates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<IActionResult> ProgressImage(IFormFile image);
        Task<IActionResult> ProgressImageForClass(Guid classId, IFormFile image);
        IActionResult UpdateClassModel(Guid classId, ClassModelCreateModel model);
    }
}
