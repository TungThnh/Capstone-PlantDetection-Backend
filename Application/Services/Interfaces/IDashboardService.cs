using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IDashboardService
    {
        IActionResult GetDashboardData();
        IActionResult GetManagerDashboardData(Guid id);
    }
}
