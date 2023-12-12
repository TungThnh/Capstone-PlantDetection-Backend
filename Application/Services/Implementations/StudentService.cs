using Application.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Constants;
using System.IO;
using Common.Extensions;
using Common.Helpers;
using Data;
using Data.Repositories.Interfaces;
using Domain.Models.Filters;
using Domain.Models.Pagination;
using Domain.Models.Updates;
using Domain.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Application.Services.Implementations
{
    public class StudentService : BaseService, IStudentService
    {
        private readonly new IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly string _connectionString;

        public StudentService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            _mapper = mapper;
            _studentRepository = unitOfWork.Student;
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? null!;
        }

        public async Task<DataTable> GetStudentsDataTableAsync(Guid classId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = _studentRepository.GetMany(st => st.StudentClass != null ? st.StudentClass.ClassId.Equals(classId) : false).ToQueryString();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        await Task.Run(() => dataAdapter.Fill(dataTable));
                        return dataTable;
                    }
                }
            }
        }

        public async Task ExportStudents(Guid classId)
        {
            try
            {
                var students = await GetStudentsDataTableAsync(classId);
                string fileName = $"{classId}.xlsx";

                // Tạo đường dẫn đến thư mục Students trên Desktop
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string savedFolderPath = Path.Combine(desktopPath + "/Students", fileName);

                ExcelHelper.ExportToExcel(students, "Students", savedFolderPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudent(Guid id)
        {
            try
            {
                var student = await _studentRepository.GetMany(st => st.Id.Equals(id))
                    .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                if (student == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }
                return new ObjectResult(student)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetStudents(PaginationRequestModel pagination, StudentFilterModel filter)
        {
            try
            {
                var query = _studentRepository.GetAll();
                if (filter.Name != null)
                {
                    query = query.Where(st => st.FirstName.Contains(filter.Name) || st.LastName.Contains(filter.Name));
                }
                var totalRow = await query.AsNoTracking().CountAsync();
                var students = await query.AsNoTracking()
                    .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
                    .OrderBy(st => st.LastName)
                    .Paginate(pagination)
                    .ToListAsync();
                return new OkObjectResult(students.ToPaged(pagination, totalRow));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> UpdateStudent(Guid id, StudentUpdateModel model)
        {
            try
            {
                var student = await _studentRepository.FirstOrDefaultAsync(st => st.Id.Equals(id));
                if (student == null)
                {
                    return new ObjectResult(CustomErrors.RecordNotFound)
                    {
                        StatusCode = StatusCodes.Status404NotFound
                    };
                }

                _mapper.Map(model, student);

                _studentRepository.Update(student);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result > 0)
                {
                    return await GetStudent(id);
                }
                return new ObjectResult(CustomErrors.UnprocessableEntity)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
