using Common.Enums;
using Domain.Models.Systems;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> AuthenticateWithGoogleAsync(GoogleIdTokenModel model, AuthenticationType type);

        Task<AuthModel> GetUser(Guid id);
    }
}
