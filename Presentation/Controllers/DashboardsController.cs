using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Systems;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/dashboards")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardsController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult GetDashboardData()
        {
            try
            {
                return _dashboardService.GetDashboardData();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(UserRoles.Manager)]
        [Route("managers")]
        public IActionResult GetManagerDashboardData()
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return _dashboardService.GetManagerDashboardData(user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
