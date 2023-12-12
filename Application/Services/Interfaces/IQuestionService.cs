using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<IActionResult> GetQuestions(QuestionFilterModel filter, PaginationRequestModel pagination);
        Task<IActionResult> GetQuestion(Guid id);
        Task<IActionResult> CreateQuestion(QuestionCreateModel model);
        Task<IActionResult> GetQuestionResult(Guid id);
    }
}