using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IActionResult> GetCategories(CategoryFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetCategory(Guid id);
    }
}
