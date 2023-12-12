using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task ExportStudents(Guid classId);
        Task<IActionResult> GetStudent(Guid id);
        Task<IActionResult> GetStudents(PaginationRequestModel pagination, StudentFilterModel filter);
        Task<IActionResult> UpdateStudent(Guid id, StudentUpdateModel model);
    }
}
