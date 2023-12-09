using Domain.Models.Creates;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IPlantService
    {
        Task<IActionResult> GetPlant(Guid id);
        Task<IActionResult> GetPlants(PaginationRequestModel pagination, PlantFilterModel filter);
        Task<IActionResult> CreatePlant(PlantCreateModel model);
    }
}
