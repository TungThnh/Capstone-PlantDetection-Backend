using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class ClassLabelRepository : Repository<ClassLabel>, IClassLabelRepository
    {
        public ClassLabelRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}