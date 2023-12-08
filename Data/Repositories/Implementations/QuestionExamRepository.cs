using Data.Repositories.Interfaces;
using Domain.Entities;

namespace Data.Repositories.Implementations
{
    public class QuestionExamRepository : Repository<QuestionExam>, IQuestionExamRepository
    {
        public QuestionExamRepository(PlantDetectionContext context) : base(context)
        {
        }
    }
}
