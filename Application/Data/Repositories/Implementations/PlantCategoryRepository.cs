using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class PlantCategoryRepository : Repository<PlantCategory>, IPlantCategoryRepository
    {
        public PlantCategoryRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
