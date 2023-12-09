using Application.Services.Interfaces;
using Common.Enums;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("google/student")]
        [SwaggerOperation(Summary = "User authentication via Google for Student role")]
        public async Task<IActionResult> StudentGoogleAuth([FromBody] GoogleIdTokenModel model)
        {
            try
            {
                return await _authService.AuthenticateWithGoogleAsync(model, AuthenticationType.ForStudent);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpPost("google/manager")]
        [SwaggerOperation(Summary = "User authentication via Google for Manager role")]
        public async Task<IActionResult> ManagerGoogleAuth([FromBody] GoogleIdTokenModel model)
        {
            try
            {
                return await _authService.AuthenticateWithGoogleAsync(model, AuthenticationType.ForManager);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }
    }
}
