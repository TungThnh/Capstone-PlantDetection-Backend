using Application.Services.Interfaces;
using Common.Constants;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Drawing;

namespace Presentation.Controllers
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    [Route("api/connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly ICloudStorageService _cloudStorageService;
        public ConnectionsController(ICloudStorageService cloudStorageService)
        {
            _cloudStorageService = cloudStorageService;
        }

        [HttpGet]
        [Route("app")]
        [SwaggerOperation(Summary = "Check if the app is working")]
        public IActionResult App()
        {
            return Ok("The application is being active");
        }

        [HttpGet]
        [Route("authenticate")]
        [Authorize]
        [SwaggerOperation(Summary = "Check if the user is authenticated")]
        public IActionResult Authenticate()
        {
            return Ok("You have been authenticated");
        }

        [HttpGet]
        [Route("authorize/manager")]
        [Authorize(UserRoles.Manager)]
        [SwaggerOperation(Summary = "Check if user is Manager")]
        public IActionResult AuthorizeManager()
        {
            return Ok("You are the manager");
        }

        [HttpGet]
        [Route("authorize/student")]
        [Authorize(UserRoles.Student)]
        [SwaggerOperation(Summary = "Check if user is Student")]
        public IActionResult AuthorizeStudent()
        {
            return Ok("You are the student");
        }

        [HttpPost]
        [Route("images")]
        public async Task<IActionResult> TestUploadImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var url = await _cloudStorageService.Upload(Guid.NewGuid(), file.ContentType, file);
                    return Ok(url);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
