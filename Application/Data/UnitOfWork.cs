using Data.Repositories.Implementations;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlantDetectionContext _context;
        private IDbContextTransaction _transaction = null!;

        public IStudentRepository _student = null!;
        public IManagerRepository _manager = null!;
        public IClassRepository _class = null!;
        public IStudentClassRepository _studentClass = null!;
        public ICategoryRepository _category = null!;
        public IPlantCategoryRepository _plantCategory = null!;
        public IPlantRepository _plant = null!;
        public IImageRepository _image = null!;
        public IReportRepository _report = null!;
        public ILabelRepository _label = null!;
        public IClassLabelRepository _classLabel = null!;
        public IQuestionRepository _question = null!;
        public IExamRepository _exam = null!;
        public IQuestionExamRepository _questionExam = null!;

        public UnitOfWork(PlantDetectionContext context)
        {
            _context = context;
        }
        public IStudentRepository Student
        {
            get { return _student ??= new StudentRepository(_context); }
        }

        public IManagerRepository Manager
        {
            get { return _manager ??= new ManagerRepository(_context); }
        }

        public IClassRepository Class
        {
            get { return _class ??= new ClassRepository(_context); }
        }

        public IStudentClassRepository StudentClass
        {
            get { return _studentClass ??= new StudentClassRepository(_context); }
        }

        public ICategoryRepository Category
        {
            get { return _category ??= new CategoryRepository(_context); }
        }  

        public IPlantCategoryRepository PlantCategory
        {
            get { return _plantCategory ??= new PlantCategoryRepository(_context); }
        }    

        public IPlantRepository Plant
        {
            get { return _plant ??= new PlantRepository(_context); }
        }    
        
        public IImageRepository Image
        {
            get { return _image ??= new ImageRepository(_context); }
        }      
        
        public IReportRepository Report
        {
            get { return _report ??= new ReportRepository(_context); }
        }   
        
        public ILabelRepository Label
        {
            get { return _label ??= new LabelRepository(_context); }
        }      
        
        public IClassLabelRepository ClassLabel
        {
            get { return _classLabel ??= new ClassLabelRepository(_context); }
        }    
        
        public IQuestionRepository Question
        {
            get { return _question ??= new QuestionRepository(_context); }
        }   
        
        public IExamRepository Exam
        {
            get { return _exam ??= new ExamRepository(_context); }
        }    
        
        public IQuestionExamRepository QuestionExam
        {
            get { return _questionExam ??= new QuestionExamRepository(_context); }
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null!;
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null!;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
