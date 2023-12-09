using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class LabelRepository : Repository<Label>, ILabelRepository
    {
        public LabelRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
