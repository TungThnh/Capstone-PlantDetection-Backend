using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IExamService
    {
        Task<IActionResult> GetExam(Guid id);
        Task<IActionResult> GetExamResult(Guid id);
        Task<IActionResult> CreateExam(Guid studentId);
        Task<IActionResult> GetStudentExams(Guid id, ExamFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> SubmitExam(Guid id, ExamSubmitCreateModel model);
        Task<IActionResult> CalculateResult(Guid id);
    }
}
