using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Systems;
using Domain.Models.Updates;
using Domain.Specifications;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about a Student by Student ID")]
        public async Task<IActionResult> GetStudent([FromRoute] Guid id)
        {
            try
            {
                return await _studentService.GetStudent(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("information")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Get information about a Student by access token")]
        public async Task<IActionResult> GetStudentByAccessToken()
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _studentService.GetStudent(user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Student list by filter condition")]
        public async Task<IActionResult> GetStudents([FromQuery] PaginationRequestModel pagination, [FromQuery] StudentFilterModel filter)
        {
            try
            {
                return await _studentService.GetStudents(pagination, filter);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update information of a Student")]
        public async Task<IActionResult> UpdateStudent(Guid id, StudentUpdateModel model)
        {
            try
            {
                return await _studentService.UpdateStudent(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}