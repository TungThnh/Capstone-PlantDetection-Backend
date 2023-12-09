using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class ExamRepository : Repository<Exam>, IExamRepository
    {
        public ExamRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
