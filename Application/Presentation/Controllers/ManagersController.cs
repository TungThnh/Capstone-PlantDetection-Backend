using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Systems;
using Domain.Models.Updates;
using Domain.Specifications;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/managers")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagersController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about a Manager by Manager ID")]
        public async Task<IActionResult> GetManager([FromRoute] Guid id)
        {
            try
            {
                return await _managerService.GetManager(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("information")]
        [Authorize(UserRoles.Manager)]
        [SwaggerOperation(Summary = "Get information about a Manager by access token")]
        public async Task<IActionResult> GetManagerByAccessToken()
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _managerService.GetManager(user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Manager list by filter condition")]
        public async Task<IActionResult> GetManagers([FromQuery] PaginationRequestModel pagination, [FromQuery] ManagerFilterModel filter)
        {
            try
            {
                return await _managerService.GetManagers(pagination, filter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update information of a Manager")]
        public async Task<IActionResult> UpdateManager(Guid id, ManagerUpdateModel model)
        {
            try
            {
                return await _managerService.UpdateManager(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}