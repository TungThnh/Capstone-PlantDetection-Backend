using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class StudentClassRepository : Repository<StudentClass>, IStudentClassRepository
    {
        public StudentClassRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
