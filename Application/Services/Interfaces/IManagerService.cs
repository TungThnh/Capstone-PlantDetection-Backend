using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IManagerService
    {
        Task<IActionResult> GetManager(Guid id);
        Task<IActionResult> GetManagers(PaginationRequestModel pagination, ManagerFilterModel filter);
        Task<IActionResult> UpdateManager(Guid id, ManagerUpdateModel model);
    }
}
