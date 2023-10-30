using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IActionResult> GetCategory(Guid id);
    }
}
