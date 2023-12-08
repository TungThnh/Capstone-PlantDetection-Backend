using Application.Services.Interfaces;
using Common.Constants;
using Common.Helpers;
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
                if (!ImageHelpers.IsImage(image))
                {
                    return new ObjectResult(CustomErrors.InvalidFileType)
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }
                return await _predictionService.ProgressImage(image);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("classes/{id}")]
        public async Task<IActionResult> ProgressImageForClass([FromRoute] Guid id, IFormFile image)
        {
            try
            {
                if (!ImageHelpers.IsImage(image))
                {
                    return new ObjectResult(CustomErrors.InvalidFileType)
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                    };
                }
                return await _predictionService.ProgressImageForClass(id, image);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [DisableRequestSizeLimit]
        [Route("model/update/class/{id}")]
        public IActionResult UpdateClassModel([FromRoute] Guid id, [FromForm] ClassModelCreateModel model)
        {
            try
            {
                string fileExtension = Path.GetExtension(model.Model.FileName);
                if (fileExtension.ToLower() != ".pt")
                {
                    return new ObjectResult(CustomErrors.InvalidFileType)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                return _predictionService.UpdateClassModel(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
