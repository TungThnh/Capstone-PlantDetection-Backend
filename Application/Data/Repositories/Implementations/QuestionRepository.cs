using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
