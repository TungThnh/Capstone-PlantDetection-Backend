using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface ILabelService
    {
        Task<IActionResult> GetLabels(LabelFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetLabel(Guid id);
        Task<IActionResult> CreateLabel(LabelCreateModel model);
        Task<IActionResult> UpdateLabel(Guid id, LabelUpdateModel model);
        Task<IActionResult> RemoveLabel(Guid id);
    }
}
