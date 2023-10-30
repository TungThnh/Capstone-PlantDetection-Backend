using Application.Services.Interfaces;
using AutoMapper;
using Data;

namespace Application.Services.Implementations
{
    public class PlantCategoryService : BaseService, IPlantCategoryService
    {
        public PlantCategoryService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
    }
}
