using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/predictions")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpPost]
        public async Task<IActionResult> ProgressImage(IFormFile image)
        {
            try
            {
                var a = image;
                return await _predictionService.ProgressImage(image);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
