using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public interface IUnitOfWork
    {
        public IStudentRepository Student { get; }
        public IManagerRepository Manager { get; }
        public IClassRepository Class { get; }
        public IStudentClassRepository StudentClass { get; }
        public ICategoryRepository Category { get; }
        public IPlantCategoryRepository PlantCategory { get; }
        public IPlantRepository Plant { get; }
        public IImageRepository Image { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Dispose();
        Task<int> SaveChangesAsync();
    }
}
