using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class PlantRepository : Repository<Plant>, IPlantRepository
    {
        public PlantRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
