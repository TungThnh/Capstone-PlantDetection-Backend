using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/labels")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Label Label list by filter condition")]
        public async Task<IActionResult> GetLabels([FromQuery] LabelFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _labelService.GetLabels(filter, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about Label by Label ID")]
        public async Task<IActionResult> GetLabel([FromRoute] Guid id)
        {
            try
            {
                return await _labelService.GetLabel(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] LabelCreateModel model)
        {
            try
            {
                return await _labelService.CreateLabel(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateLabel([FromRoute] Guid id, [FromBody] LabelUpdateModel model)
        {
            try
            {
                return await _labelService.UpdateLabel(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteLabel([FromRoute] Guid id)
        {
            try
            {
                return await _labelService.RemoveLabel(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
