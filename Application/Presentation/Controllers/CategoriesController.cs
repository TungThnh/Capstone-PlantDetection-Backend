using Application.Services.Interfaces;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Plant Category list by filter condition")]
        public async Task<IActionResult> GetCategories([FromQuery] CategoryFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _categoryService.GetCategories(filter, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about Category by Category ID")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            try
            {
                return await _categoryService.GetCategory(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
