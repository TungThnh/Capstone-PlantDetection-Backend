using Application.Services.Interfaces;
using Common.Constants;
using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Systems;
using Domain.Models.Updates;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get information about a Class by Class ID")]
        public async Task<IActionResult> GetClass([FromRoute] Guid id)
        {
            try
            {
                return await _classService.GetClass(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("student")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Get Class information by Student access token")]
        public async Task<IActionResult> GetStudentClass()
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _classService.GetStudentClass(user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get information about Class list by filter condition")]
        public async Task<IActionResult> GetClasses([FromQuery] PaginationRequestModel pagination, [FromQuery] ClassFilterModel model)
        {
            try
            {
                return await _classService.GetClasses(pagination, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/students")]
        [SwaggerOperation(Summary = "Get the list of Students of a Class by the Class ID")]
        public async Task<IActionResult> GetClassStudents([FromRoute] Guid id, [FromQuery] PaginationRequestModel pagination)
        {
            try
            {
                return await _classService.ListOfStudents(id, pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("code/{code}")]
        [SwaggerOperation(Summary = "Find class by Class code")]
        //[Authorize(UserRoles.Student)]
        public async Task<IActionResult> FindClassByCode([FromRoute] string code)
        {
            try
            {
                return await _classService.FindClassByCode(code);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(UserRoles.Manager)]
        [SwaggerOperation(Summary = "Create a new class by Manager")]
        public async Task<IActionResult> CreateClass([FromBody] ClassCreateModel model)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _classService.CreateClass(model, user.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update information of a Class")]
        public async Task<IActionResult> UpdateClass([FromRoute] Guid id, ClassUpdateModel model)
        {
            try
            {
                return await _classService.UpdateClass(id, model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("request-to-join")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Students submit a request to join the Class")]
        public async Task<IActionResult> RequestToJoinClass([FromQuery] Guid classId)
        {
            try
            {
                var user = (AuthModel)HttpContext.Items["User"]!;
                return await _classService.RequestToJoinClass(user.Id, classId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("invite-student")]
        [Authorize(UserRoles.Manager)]
        [SwaggerOperation(Summary = "Manager invite Student to join the Class")]
        public async Task<IActionResult> InviteStudentIntoClass([FromBody] InviteStudentModel model)
        {
            try
            {
                return await _classService.InviteStudentIntoClass(model);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
