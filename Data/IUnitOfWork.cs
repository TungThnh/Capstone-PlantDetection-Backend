using Data.Repositories.Interfaces;

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
        public IReportRepository Report { get; }
        public ILabelRepository Label { get; }
        public IClassLabelRepository ClassLabel { get; }
        public IQuestionRepository Question { get; }
        public IExamRepository Exam { get; }
        public IQuestionExamRepository QuestionExam { get; }

        void BeginTransaction();
        void Commit();
        void Rollback();
        void Dispose();
        Task<int> SaveChangesAsync();
    }
}