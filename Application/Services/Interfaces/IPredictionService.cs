using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<IActionResult> ProgressImage(IFormFile image);
    }
}
