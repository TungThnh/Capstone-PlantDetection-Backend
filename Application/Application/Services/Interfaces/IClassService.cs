using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IClassService
    {
        Task<IActionResult> GetClass(Guid id);
        Task<IActionResult> GetAvailableClasses(PaginationRequestModel pagination, ClassFilterModel filter);
        Task<IActionResult> GetClasses(PaginationRequestModel pagination, ClassFilterModel filter);
        Task<IActionResult> GetManagerClasses(Guid id, ClassFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetStudentClass(Guid id);
        Task<IActionResult> FindClassByCode(string code);
        Task<IActionResult> IsClassModelExists(Guid id);
        Task<IActionResult> CreateClass(ClassCreateModel model, Guid managerId);
        Task<IActionResult> UpdateClass(Guid id, ClassUpdateModel model);
        Task<IActionResult> ApproveStudentToJoinClass(Guid classId, Guid studentId);
        Task<IActionResult> LeaveClass(Guid classId, Guid studentId);
        Task<IActionResult> RequestToJoinClass(Guid studentId, Guid classId);
        Task<IActionResult> InviteStudentIntoClass(InviteStudentModel model);
        Task<IActionResult> ListOfStudents(Guid id, PaginationRequestModel pagination);
    }
}
