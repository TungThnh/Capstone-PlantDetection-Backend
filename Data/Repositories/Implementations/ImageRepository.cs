using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
