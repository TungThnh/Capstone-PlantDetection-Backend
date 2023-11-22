using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IActionResult> CreateReport(Guid studentId, ReportCreateModel model);
        Task<IActionResult> GetManagerReports(Guid managerId, ReportFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetReport(Guid id);
        Task<IActionResult> GetReports(PaginationRequestModel pagination, ReportFilterModel filter);
        Task<IActionResult> UpdateReport(Guid id, ReportUpdateModel model);
    }
}