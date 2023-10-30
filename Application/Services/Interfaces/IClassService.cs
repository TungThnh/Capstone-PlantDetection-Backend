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
        Task<IActionResult> GetClasses(PaginationRequestModel pagination, ClassFilterModel filter);
        Task<IActionResult> GetStudentClass(Guid id);
        Task<IActionResult> FindClassByCode(string code);
        Task<IActionResult> CreateClass(ClassCreateModel model, Guid managerId);
        Task<IActionResult> UpdateClass(Guid id, ClassUpdateModel model);
        Task<IActionResult> RequestToJoinClass(Guid studentId, Guid classId);
        Task<IActionResult> InviteStudentIntoClass(InviteStudentModel model);
        Task<IActionResult> ListOfStudents(Guid id, PaginationRequestModel pagination);
    }
}
