using Application.Services.Interfaces;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about question list by filter condition")]
        public async Task<IActionResult> GetQuestions([FromQuery] QuestionFilterModel filter, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _questionService.GetQuestions(filter, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about Question by Question ID")]
        public async Task<IActionResult> GetQuestion([FromRoute] Guid id)
        {
            try
            {
                return await _questionService.GetQuestion(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/result")]
        [SwaggerOperation(Summary = "Get result of the question")]
        public async Task<IActionResult> GetQuestionResult([FromRoute] Guid id)
        {
            try
            {
                return await _questionService.GetQuestionResult(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromForm] QuestionCreateModel model)
        {
            try
            {
                return await _questionService.CreateQuestion(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
