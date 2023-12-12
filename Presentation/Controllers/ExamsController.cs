using Application.Services.Implementations;
using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Systems;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/exams")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IExamService _examService;
        public ExamsController(IExamService examService)
        {
            _examService = examService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about a exam by exam Id")]
        public async Task<IActionResult> GetExam([FromRoute] Guid id)
        {
            try
            {
                return await _examService.GetExam(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/results")]
        [SwaggerOperation(Summary = "Get information about a exam result by exam Id")]
        public async Task<IActionResult> GetExamResult([FromRoute] Guid id)
        {
            try
            {
                return await _examService.GetExamResult(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("students")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Get student exams history")]
        public async Task<IActionResult> GetStudentExams([FromQuery] ExamFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _examService.GetStudentExams(user.Id, filter, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/calculate-score")]
        [SwaggerOperation(Summary = "Get score of the exam")]
        public async Task<IActionResult> GetExamsScore([FromRoute] Guid id)
        {
            try
            {
                return await _examService.CalculateResult(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Create a new exam")]
        public async Task<IActionResult> CreateExam()
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _examService.CreateExam(user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Submit the exam")]
        public async Task<IActionResult> SubmitExam([FromRoute] Guid id, [FromBody] ExamSubmitCreateModel model)
        {
            try
            {
                return await _examService.SubmitExam(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
