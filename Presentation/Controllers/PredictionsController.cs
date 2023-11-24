using Application.Services.Interfaces;
using Domain.Models.Creates;
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
                return await _predictionService.ProgressImage(image);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("model/update/class/{id}")]
        public IActionResult UpdateClassModel([FromRoute] Guid id, [FromForm] ClassModelCreateModel model)
        {
            try
            {
                return _predictionService.UpdateClassModel(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}