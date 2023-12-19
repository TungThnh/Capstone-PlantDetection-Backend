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
    [Route("api/plants")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly IPlantService _plantService;

        public PlantsController(IPlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about Plant by Plant ID")]
        public async Task<IActionResult> GetPlant([FromRoute] Guid id)
        {
            try
            {
                return await _plantService.GetPlant(id);
            }
            catch (Exception ex)    
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Plant list by filter condition")]
        public async Task<IActionResult> GetPlants([FromQuery] PaginationRequestModel pagination, [FromQuery] PlantFilterModel filter)
        {
            try
            {
                return await _plantService.GetPlants(pagination, filter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlant([FromForm] PlantCreateModel model)
        {
            try
            {
                return await _plantService.CreatePlant(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update information of a plant")]
        public async Task<IActionResult> UpdatePlant(Guid id, PlantUpdateModel model)
        {
            try
            {
                return await _plantService.UpdatePlant(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
