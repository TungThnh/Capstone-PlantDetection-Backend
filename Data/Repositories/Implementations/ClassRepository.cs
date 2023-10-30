using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        public ClassRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
