using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Systems;
using Domain.Models.Updates;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about Report by Report ID")]
        public async Task<IActionResult> GetReport([FromRoute] Guid id)
        {
            try
            {
                return await _reportService.GetReport(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("students")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Get information about report list of student by filter condition")]
        public async Task<IActionResult> GetStudentReports([FromQuery] PaginationRequestModel pagination, [FromQuery] ReportFilterModel filter)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _reportService.GetStudentReports(user.Id, pagination, filter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Report list by filter condition")]
        public async Task<IActionResult> GetReports([FromQuery] PaginationRequestModel pagination, [FromQuery] ReportFilterModel filter)
        {
            try
            {
                return await _reportService.GetReports(pagination, filter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("manager")]
        [SwaggerOperation(Summary = "Get information about Report list by manager access token")]
        public async Task<IActionResult> GetManagerReports([FromQuery] ReportFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _reportService.GetManagerReports(user.Id, filter, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(UserRoles.Student)]
        public async Task<IActionResult> CreateReport([FromForm] ReportCreateModel model)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _reportService.CreateReport(user.Id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateReport([FromRoute] Guid id, [FromBody] ReportUpdateModel model)
        {
            try
            {
                return await _reportService.UpdateReport(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
